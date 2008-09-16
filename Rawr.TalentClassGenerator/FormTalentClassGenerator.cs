using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Rawr.TalentClassGenerator
{
	public partial class FormTalentClassGenerator : Form
	{
		public FormTalentClassGenerator()
		{
			InitializeComponent();
		}

		private void buttonGenerateCode_Click(object sender, EventArgs e)
		{
			textBoxCode.Text = @"using System;
using System.Text;
using System.Collections.Generic;

namespace Rawr
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class TalentDataAttribute : Attribute
	{
		public TalentDataAttribute(int index, string name, int maxPoints, int tree, int column, int row, int prerequisite, string[] description)
		{
			_index = index;
			_name = name;
			_maxPoints = maxPoints;
			_tree = tree;
			_column = column;
			_row = row;
			_prerequisite = prerequisite;
			_description = description;
		}

		private readonly int _index;
		private readonly string _name;
		private readonly int _maxPoints;
		private readonly int _tree;
		private readonly int _column;
		private readonly int _row;
		private readonly int _prerequisite;
		private readonly string[] _description;

		public int Index { get { return _index; } }
		public string Name { get { return _name; } }
		public int MaxPoints { get { return _maxPoints; } }
		public int Tree { get { return _tree; } }
		public int Column { get { return _column; } }
		public int Row { get { return _row; } }
		public int Prerequisite { get { return _prerequisite; } }
		public string[] Description { get { return _description; } }
	}

";
			foreach (string className in new string[] { "priest", "mage", "warlock", 
				"druid", "rogue", "hunter", "shaman", "paladin", "warrior", "deathknight" })
				ProcessTalentDataJS(new StreamReader(System.Net.HttpWebRequest.Create(
					string.Format(textBoxUrl.Text, className)).GetResponse().GetResponseStream()).ReadToEnd());
			textBoxCode.Text += "}";
			textBoxCode.SelectAll();
			textBoxCode.Focus();
		}

		private void ProcessTalentDataJS(string fullResponse)
		{
			string className = string.Empty;
			List<string> treeNames = new List<string>();
			List<TalentData> talents = new List<TalentData>();
			int descriptionIndex = 0;
			//foreach (string line in fullResponse.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))

			string[] lines = fullResponse.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			for (int iLine = 0; iLine < lines.Length; iLine++)
			{
				string line = lines[iLine];
				
				if (line.StartsWith("var className"))
				{
					className = GetTextBetween(line, "\"", "\"").Replace(" ", "");
				}
				else if (line.StartsWith("tree[i] = "))
				{
					treeNames.Add(GetTextBetween(line, "\"", "\""));
				}
				else if (line.StartsWith("talent[i] = "))
				{
					string talentString = GetTextBetween(line, " [", "];"); 
					//0  1                 2  3  4  5          0=tree, 1=name, 2=maxpoints, 3=column, 4=row, 5=prereq
					//0, "Nature's Grace", 1, 2, 3, [getTalentID("Nature's Mastery"),2]
					talentString = talentString.Replace(GetTextBetween(talentString, "\"", "\""), GetTextBetween(talentString, "\"", "\"").Replace(",", "|")); // Replace ,'s in talent names with |'s
					if (talentString.Contains("getTalentID")) talentString = talentString.Replace(GetTextBetween(talentString, "getTalentID(\"", "\")"), GetTextBetween(talentString, "getTalentID(\"", "\")").Replace(",", "|")); // Replace ,'s in talent names with |'s
					string[] talentStringArray = talentString.Split(new string[] { ", " }, StringSplitOptions.None);
					TalentData talentData = new TalentData()
					{
						Index = talents.Count,
						Name = talentStringArray[1].Trim('"').Replace("|",","), //put the ,'s back in place of the |'s
						MaxPoints = int.Parse(talentStringArray[2]),
						Tree = int.Parse(talentStringArray[0]),
						Column = int.Parse(talentStringArray[3]),
						Row = int.Parse(talentStringArray[4].TrimEnd(',')),
						Prerequisite = -1,
						Description = new string[0]
					};
					if (talentStringArray.Length > 5)
					{ //has prereqs
						if (talentStringArray[5].StartsWith("[i+"))
						{
							talentData.Prerequisite = talentData.Index + int.Parse(GetTextBetween(talentStringArray[5], "+", ","));
						}
						else
						{
							string prereqName = GetTextBetween(talentStringArray[5], "\"", "\"").Replace("|", ","); //put the ,'s back in place of the |'s
							foreach (TalentData otherTalent in talents)
								if (otherTalent.Name == prereqName)
								{
									talentData.Prerequisite = otherTalent.Index;
								}
						}
					}
					talents.Add(talentData);
				}
				else if (line.StartsWith("rank[i]"))
				{
					List<string> descRanks = new List<string>();
					for (int endLine = iLine + 1; lines[endLine].Trim().StartsWith("\""); endLine++)
					{
						string rankLine = lines[endLine];
						while (rankLine.EndsWith("\\")) rankLine = rankLine.TrimEnd('\\') + lines[++endLine];
						descRanks.Add(GetTextBetween(rankLine, "\"", "\"").Replace("<span style=text-align:left;float:left;>","").Replace("</span>",",")
							.Replace("<span style=text-align:right;float:right;>", "").Replace("<br>", "\r\n").Replace("<BR>", "\r\n").Replace("&nbsp;"," "));
					}
					talents[descriptionIndex++].Description = descRanks.ToArray();
				}
			}

			//Generate the code
			StringBuilder code = new StringBuilder();
			code.AppendFormat("public class {0} : ICloneable\r\n", className);
			code.Append("{\r\n");
			code.AppendFormat("private int[] _data = new int[{0}];\r\n", talents.Count);
			code.AppendFormat("public {0}() {{ }}\r\n", className);
			code.AppendFormat("public {0}(string talents)\r\n", className);
			code.Append("{\r\n");
			code.Append("List<int> data = new List<int>();\r\n");
			code.Append("foreach (Char digit in talents)\r\n");
			code.Append("data.Add(int.Parse(digit.ToString()));\r\n");
			code.Append("data.CopyTo(_data);\r\n");
			code.Append("}\r\n");
			code.AppendFormat("\r\npublic override string ToString()\r\n", className);
			code.Append("{\r\n");
			code.Append("StringBuilder ret = new StringBuilder();\r\n");
			code.Append("foreach (int digit in _data)\r\n");
			code.Append("ret.Append(digit.ToString());\r\n");
			code.Append("return ret.ToString();\r\n");
			code.Append("}\r\n");
            code.Append("object ICloneable.Clone()\r\n");
            code.Append("{\r\n");
            code.AppendFormat("{0} clone = ({0})MemberwiseClone();\r\n", className);
            code.Append("clone._data = (int[])_data.Clone();\r\n");
            code.Append("return clone;\r\n");
            code.Append("}\r\n\r\n");
            code.AppendFormat("public {0} Clone()\r\n", className);
            code.Append("{\r\n");
            code.AppendFormat("return ({0})((ICloneable)this).Clone();\r\n", className);
            code.Append("}\r\n\r\n");
			foreach (TalentData talent in talents)
			{
				code.AppendFormat("\r\n[TalentData({0}, \"{1}\", {2}, {3}, {4}, {5}, {6}, new string[] {{",
					talent.Index, talent.Name, talent.MaxPoints, talent.Tree, talent.Column, talent.Row, talent.Prerequisite);
				foreach (string descRank in talent.Description)
					code.AppendFormat("\r\n@\"{0}\",", descRank);
				code.Append("})]\r\n");
				code.AppendFormat("public int {0} {{ get {{ return _data[{1}]; }} set {{ _data[{1}] = value; }} }}\r\n", 
					PropertyFromName(talent.Name), talent.Index);				
			}
			code.Append("}\r\n\r\n");
			
			textBoxCode.Text += code.ToString();
		}

        private string PropertyFromName(string name)
        {
            name = name.Replace("'", ""); // don't camel word after apostrophe
            string[] arr = name.Split(new char[] {' ', ',', ':', '(', ')', '.', '-'}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = Char.ToUpperInvariant(arr[i][0]) + arr[i].Substring(1);
            }
            return string.Join("", arr);
        }

        private string GetTextBetween(string text, string start, string end)
		{
			string ret = text.Substring(text.IndexOf(start) + start.Length);
			ret = ret.Substring(0, ret.IndexOf(end));
			return ret;
		}

		private class TalentData
		{
			public int Index { get; set; }
			public string Name { get; set; }
			public int MaxPoints { get; set; }
			public int Tree { get; set; }
			public int Column { get; set; }
			public int Row { get; set; }
			public int Prerequisite { get; set; }
			public string[] Description { get; set; }
		}
	}


	/*
	 talent[i] = [0, "Starlight Wrath", 5, 2, 1]; i++;
	talent[i] = [0, "Genesis", 5, 3, 1]; i++;
	talent[i] = [0, "Moonglow", 3, 1, 2]; i++;
	talent[i] = [0, "Nature's Mastery", 2, 2, 2]; i++;
	talent[i] = [0, "Improved Moonfire", 2, 4, 2]; i++;
	talent[i] = [0, "Brambles", 3, 1, 3]; i++;
	 * */

	public class DruidTalentArray
	{
		private int[] _data = new int[50];

		private DruidTalentArray() { }
		private DruidTalentArray(string talents)
		{
			List<int> data = new List<int>();
			foreach (Char digit in talents)
				data.Add(int.Parse(digit.ToString()));
			data.CopyTo(_data);
		}

		[TalentData(0, "Starlight Wrath", 5, 0, 2, 1, -1, new string[] {
			"StarA",
			"StarB",
			"StarC",})]
		public int StarlightWrath { get { return _data[0]; } set { _data[0] = value; } }



	}

	[global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class TalentDataAttribute : Attribute
	{
		public TalentDataAttribute(int index, string name, int maxPoints, int tree, int column, int row, int prerequisite, string[] description)
		{
			_index = index;
			_name = name;
			_maxPoints = maxPoints;
			_tree = tree;
			_column = column;
			_row = row;
			_prerequisite = prerequisite;
			_description = description;
		}

		private readonly int _index;
		private readonly string _name;
		private readonly int _maxPoints;
		private readonly int _tree;
		private readonly int _column;
		private readonly int _row;
		private readonly int _prerequisite;
		private readonly string[] _description;

		public int Index { get { return _index; } }
		public string Name { get { return _name; } }
		public int MaxPoints { get { return _maxPoints; } }
		public int Tree { get { return _tree; } }
		public int Column { get { return _column; } }
		public int Row { get { return _row; } }
		public int Prerequisite { get { return _prerequisite; } }
		public string[] Description { get { return _description; } }
	}
}
