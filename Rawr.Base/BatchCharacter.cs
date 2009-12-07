using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace Rawr
{
    public class BatchCharacter : INotifyPropertyChanged
    {
        private string relativePath;
        public string RelativePath
        {
            get
            {
                return relativePath;
            }
            set
            {
                if (relativePath != value)
                {
                    relativePath = value;
                    character = null;

                    string curDir = Directory.GetCurrentDirectory();
                    Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                    absolutePath = Path.GetFullPath(relativePath);
                    Directory.SetCurrentDirectory(curDir);

                    score = Optimizer.ItemInstanceOptimizer.GetOptimizationValue(Character, Model);

                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        private string absolutePath;
        [XmlIgnore]
        public string AbsulutePath
        {
            get
            {
                return absolutePath;
            }
        }

        [XmlIgnore]
        public string Name
        {
            get
            {
                if (relativePath == null) return null;
                string name = Path.GetFileNameWithoutExtension(relativePath);
                if (unsavedChanges) name += " *";
                return name;
            }
        }

        private Character character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                if (character == null && absolutePath != null)
                {
                    character = Character.Load(absolutePath);
                    character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                }
                return character;
            }
        }

        void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            unsavedChanges = true;
            newScore = Optimizer.ItemInstanceOptimizer.GetOptimizationValue(Character, Model);

            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewScore"));
        }

        private CalculationsBase model;
        [XmlIgnore]
        public CalculationsBase Model
        {
            get
            {
                if (model == null || model.Name != Character.CurrentModel)
                {
                    model = Calculations.GetModel(Character.CurrentModel);
                }
                return model;
            }
        }

        private bool unsavedChanges;
        [XmlIgnore]
        public bool UnsavedChanges
        {
            get
            {
                return unsavedChanges;
            }
            set
            {
                if (unsavedChanges != value)
                {
                    if (!value)
                    {
                        if (newScore != null)
                        {
                            score = (float)newScore;
                            newScore = null;
                        }
                    }
                    unsavedChanges = value;
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                    if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("NewScore"));
                }
            }
        }

        private float weight = 1f;
        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (weight != value)
                {
                    weight = value;
                }
            }
        }

        public bool Locked { get; set; }

        private float score;
        [XmlIgnore]
        public float Score
        {
            get
            {
                return score;
            }
        }

        private float? newScore;
        [XmlIgnore]
        public float? NewScore
        {
            get
            {
                return newScore;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class BatchCharacterList : BindingList<BatchCharacter>
    {
        public static BatchCharacterList Load(string path)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchCharacterList));
            System.IO.StreamReader reader = new System.IO.StreamReader(path);
            BatchCharacterList list = (BatchCharacterList)serializer.Deserialize(reader);
            reader.Close();
            return list;
        }

        public void Save(string path)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchCharacterList));
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path);
            serializer.Serialize(writer, this);
            writer.Close();
        }

        public List<KeyValuePair<Character, float>> GetBatchOptimizerList()
        {
            List<KeyValuePair<Character, float>> list = new List<KeyValuePair<Character, float>>();
            foreach (BatchCharacter batchCharacter in this)
            {
                list.Add(new KeyValuePair<Character, float>(batchCharacter.Character, batchCharacter.Weight));
            }
            return list;
        }

        public float NewScore
        {
            get
            {
                float score = 0.0f;
                foreach (BatchCharacter character in this)
                {
                    score += character.Weight * (character.NewScore ?? character.Score);
                }
                return score;
            }
        }

        public float Score
        {
            get
            {
                float score = 0.0f;
                foreach (BatchCharacter character in this)
                {
                    score += character.Weight * character.Score;
                }
                return score;
            }
        }
    }
}
