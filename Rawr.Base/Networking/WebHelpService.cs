using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Reflection;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif
using System.Text;

/*
 * This site pulls a regular character.xml file. Astrylian is owner and writer
 * of the site so he's just giving us data exactly like we need it
*/
namespace Rawr
{
    public class WebHelpService
    {
        public const string URL_WEBHELP = "http://rawr.codeplex.com/wikipage?title={0}";
        private WebClient _webClient;
        private string _lastIdentifier = "";

        public WebHelpService()
        {
            _webClient = new WebClient();
            _webClient.Encoding = Encoding.UTF8;
        }

        private void _webClient_DownloadStringCompleted_Child(object sender, DownloadStringCompletedEventArgs e, bool dowebdata)
        {
            // Handle Cancel
            if (e.Cancelled) { return; }
            // Handle Errors
            if (e.Error != null) {
                if (e.Error.Message.Contains("NotFound") || e.Error.Message.Contains("Not Found")) {
                    new Base.ErrorBox("Problem Getting Web Help from Rawr Website",
                        "The web page was not found",
                        "Try checking internet connectivity").Show();
                } else {
                    new Base.ErrorBox() {
                        Title = "Problem Getting Web Help from Rawr Website",
                        Function = "_webClient_DownloadStringCompleted(string input)",
                        TheException = e.Error,
                    }.Show();
                }
                return;
            }

            // Read in the document
            string hdoc;
            using (StringReader sr = new StringReader(e.Result))
            {
                hdoc = sr.ReadToEnd();
            }

            // Cull it down to what it actually needs to return
            {
                string startText = "<div class=\"wikidoc\">";
                string endText = "</div><div class=\"ClearBoth\"></div>";
                int startPoint = hdoc.IndexOf(startText);
                int endPoint = hdoc.IndexOf(endText, startPoint + startText.Length);
                hdoc = hdoc.Substring(startPoint + startText.Length, endPoint - (startPoint + startText.Length) - endText.Length);
            }

            if (dowebdata) {
                #region === Give it new html headers ===
                /* NOTE: The TextBox control and RichTextBox controls don't work with this :/
                 * The only way found to display it using the HTML is to make an iFrame and hover it over the app or to use a third party control*/
                #region StyleSheet
                string css =
   "<style type=\"text/css\"" + @">
html, form, form.table {
	height: 100%;
	min-width: 970px;
}
body {
	height: 100%;
	background-color: #fff;
	font-style: normal;
	font-size: .8em;
	margin: 0;
	position: relative;
}
html.Opera, .Opera form, .Opera form.table, .Opera body {
	height: 100%;
}
.Opera form, .IE form {
	margin: 0;
}
.IE6 .MinWidthDiv {
	height: 1px;
	width: 970px;
	overflow: hidden;
}
.IE6 .MinWidthContent {
	margin-top: -1px;
}
.IE table.MinWidthContent {
	table-layout: fixed;
}
hr {
	margin-top: .7em;
	margin-bottom: .7em;
	margin-top: 1em;
	margin-bottom: 1em;
}
img {
	border: 0;
}
ul {
	margin-left: 0em;
	padding-left: 2em;
	list-style-type: square;
	list-style-image: url('http://i2.codeplex.com/Images/v17672/bullet_square.gif');
}
li {
	margin-left: 0;
	margin-bottom: .1em;
	margin-top: .1em;
}
th {
	font-weight: normal;
	vertical-align: bottom;
}
.FloatRight {
	float: right;
}
.FloatLeft {
	float: left;
}
.FloatNone {
	float: none;
}
.ClearLeft {
	clear: left;
}
.ClearRight {
	clear: right;
}
.ClearBoth {
	clear: both;
}
.Block {
	display: block;
}
.NoTopMargin {
	margin-top: 0px;
}
.Inline {
	display: inline;
}
.IE .Fixed {
	table-layout: fixed;
}
.IE7 .ZoomFix {
	zoom: 100%;
}
.Separator {
	width: 95%;
	color: #bbb;
}
.Opera .ajax__calendar_container {
	z-index: 1;
}
.FF .FixWidthDiv, .Opera .FixWidthDiv {
	overflow: hidden;
}
body, input, select, textarea {
	font-family: "+"\"Segoe UI\""+","+"\"Microsoft Sans Serif\""+@",Arial,Geneva,Sans-Serif;
	color: #30332d;
}
input, select, textarea {
	font-size: 1em;
}
p {
	margin-top: 1em;
	margin-bottom: 1em;
}
.Opera wbr:after {
	content: " + "\"\00200B\"" + @";
}
.MonoSpace {
	font-family: Consolas,"+"\"Courier New\""+@",Courier,Monospace;
}
.ProperCase {
	text-transform: capitalize;
}
.NoWrap {
	white-space: nowrap;
}
.EmptyTextBox {
	color: #3E62A6 !important;
	font-style: italic;
}
.HeaderDarkText {
	color: #30332D !Important;
}
a, a:link, a:visited {
	text-decoration: underline;
	color: #3E62A6;
}
a:active {
	color: #3E62A6;
}
a:hover {
	color: #CE8B10;
}
.SecondaryText a, .SecondaryText a:active, .SecondaryText a:link, .SecondaryText a:visited, a SecondaryText, a:active.SecondaryText, a:link.SecondaryText, a:visited.SecondaryText {
	color: #56862e;
}
.NoUnderline, .NoUnderline a:active, .NoUnderline a:link, .NoUnderline a:visited {
	text-decoration: none !important;
}
.NoUnderlineGrayText, .NoUnderlineGrayText a:active, .NoUnderlineGrayText a:link, .NoUnderlineGrayText a:visited, .NoUnderlineGrayText a:hover {
	text-decoration: none !important;
	color: Gray !important;
}
.UnderlineHover, .UnderlineHover a:active, .UnderlineHover a:link, .UnderlineHover a:visited {
	text-decoration: none !important;
}
.UnderlineHover a:hover {
	text-decoration: underline !important;
}
a.disabled, a.disabled:hover, a.disabled:active, a.disabled:link, a.disabled:visited {
	text-decoration: none;
	cursor: text;
	color: #777;
}
.SiteHeader, .SiteHeader a, .SiteHeader a:link, .SiteHeader a:visited {
	color: #fff;
}
.BrowseDirectoryLink {
	white-space: nowrap;
	text-align: left !important;
	margin: 0.25em 0 0 0;
	font-size: 0.95em;
}
.BrowseDirectoryLink a, .BrowseDirectoryLink a:link {
	color: #3E62A6 !important;
}
.BrowseDirectoryLink a:hover {
	color: #CE8B10 !important;
}
.SiteHeader a:hover {
	color: #ccc;
}
.NoImages .SiteHeaderLeft a, .NoImages .SiteHeaderLeft a:active, .NoImages .SiteHeaderLeft a:link, .NoImages .SiteHeaderLeft a:visited, .NoImages .SiteHeaderLeft a:hover {
	color: #27602E;
	padding-left: .7em;
	text-decoration: none;
	font-size: 3em;
	font-weight: bold;
}
.NoImages .SiteHeaderLeft {
	padding-top: 1.2em;
}
.BulletLink {
	color: #3E62A6;
	background-image: url('http://i2.codeplex.com/Images/v17672/bullet_arrow.gif');
	background-repeat: no-repeat;
	background-position: left center;
	padding-left: 18px;
	margin-right: .4em;
}
.BulletLink a {
	color: #3E62A6;
}
.BulletLink a:hover {
	color: #CE8B10;
}
.SingleLineTextBox {
	color: #333;
	border: solid .1em #A5ACB2;
	vertical-align: middle;
	margin-right: .2em;
}
.VerticalAlignMiddle {
	vertical-align: middle;
}
.MultilineTextBox {
	color: #333;
	border: solid .1em #A5ACB2;
	width: 99%;
	height: 20em;
	vertical-align: middle;
	padding: .13em;
}
.Bold {
	font-weight: bold;
}
.BoldHighlightItem {
	font-weight: bold;
	background-color: Yellow;
}
.HighlightItem {
	background-color: #fff999;
}
.Normal {
	font-weight: normal;
}
.Italic {
	font-style: italic;
}
.SubText {
	color: #666;
	font-size: .9em;
}
a.SubLink, .SubLink a {
	color: #3E62A6;
	font-weight: normal;
	font-size: .9em;
}
a.SubLink:hover, .SubLink a:hover {
	color: #CE8B10;
}
.SubTextIndent {
	padding: 0 .4em 0 .4em;
}
.CheckBox {
	width: 1em;
	padding: 0;
}
button, .Button, .StretchButton, .DefaultButton {
	display: inline-block;
	background-color: #fff;
	background-image: url(http://i2.codeplex.com/Images/v17672/button_gradient.gif);
	background-repeat: repeat-x;
	background-position: bottom;
	width: 7.5em;
	padding: .3em;
}
.IE button, .IE .Button, .IE .StretchButton, .IE .DefaultButton {
	padding: .3em 0;
}
.IE9 button, .IE9 .Button, .IE9 .StretchButton, .IE9 .DefaultButton {
	padding: .3em .3em;
}
.StretchButton {
	width: auto;
}
.DefaultButton {
	border-color: #A5C2EE;
	border-light-color: #A5C2EE;
	border-dark-color: #6983BE;
}
.Opera .DefaultButton {
	border: outset .15em #A5C2EE;
}
.StandardPadding, table.StandardPadding td, table.StandardPadding th {
	padding: .2em .7em;
}
.Opera .StandardPadding, .Safari .StandardPadding {
	padding: 0;
}
.StandardPaddingBottom, table.StandardPaddingBottom td, table.StandardPaddingBottom th {
	padding-bottom: .2em;
}
.StandardPaddingTop, table.StandardPaddingTop td, table.StandardPaddingTop th {
	padding-top: .2em;
}
.StandardPaddingLeft, table.StandardPaddingLeft td, table.StandardPaddingLeft th {
	padding-left: .7em;
}
.StandardPaddingRight, table.StandardPaddingRight td, table.StandardPaddingRight th {
	padding-right: .7em;
}
.DoublePadding, table.DoublePadding td, table.DoublePadding th {
	padding: .4em 1.4em;
}
.DoublePaddingBottom, table.DoublePaddingBottom td, table.DoublePaddingBottom th {
	padding-bottom: .4em;
}
.DoublePaddingTop, table.DoublePaddingTop td, table.DoublePaddingTop th {
	padding-top: .4em;
}
.DoublePaddingLeft, table.DoublePaddingLeft td, table.DoublePaddingLeft th {
	padding-left: 1.4em;
}
.DoublePaddingRight, table.DoublePaddingRight td, table.DoublePaddingRight th {
	padding-right: 1.4em;
}
.HalfPadding, table.HalfPadding td, table.HalfPadding th {
	padding: .1em .35em;
}
.HalfPaddingBottom, table.HalfPaddingBottom td, table.HalfPaddingBottom th {
	padding-bottom: .1em;
}
.HalfPaddingTop, table.HalfPaddingTop td, table.HalfPaddingTop th {
	padding-top: .1em;
}
.HalfPaddingLeft, table.HalfPaddingLeft td, table.HalfPaddingLeft th {
	padding-left: .35em;
}
.HalfPaddingRight, table.HalfPaddingRight td, table.HalfPaddingRight th {
	padding-right: .35em;
}
td.VerticalPadding {
	height: .5em;
}
td.HorizontalListPadding {
	width: 2.5em;
	text-align: center;
}
span.HorizontalListPadding {
	margin-left: 2.5em;
}
.IE7 span.HorizontalListPadding {
	zoom: 100%;
}
.NoPadding {
	padding: 0 !important;
}
.StandardMargin {
	margin-left: .7em;
	margin-bottom: .7em;
}
.StandardMarginTop {
	margin-top: .7em;
}
.StandardMarginBottom {
	margin-bottom: .7em;
}
.StandardMarginLeft {
	margin-left: .7em;
}
.StandardMarginRight {
	margin-right: .7em;
}
.HalfMarginTop {
	margin-top: .3em;
}
.HalfMarginBottom {
	margin-bottom: .3em;
}
.HalfMarginLeft {
	margin-left: .3em;
}
.NoMargin {
	margin: 0;
}
.NoBorder {
	border: none !important;
}
.VerticalBar {
	color: #777;
	display: inline;
	padding-left: 1em;
	padding-right: 1em;
	font-size: 1em;
	vertical-align: text-top;
	overflow: hidden;
}
.ClearBoth {
	clear: both;
}
.BulletError {
	font-size: .9em;
	font-weight: bold;
	color: #F00;
	background-image: url(http://i2.codeplex.com/Images/v17672/cautionsign_bug.gif);
	background-position: left top;
	background-repeat: no-repeat;
	padding-left: 22px;
}
.BulletErrorExtra {
	font-weight: bold;
	background-position: left top;
	background-repeat: no-repeat;
}
div.BulletError {
	margin-top: .7em;
	margin-bottom: .7em;
}
.DottedLine {
	background-image: url(http://i2.codeplex.com/Images/v17672/underline_dot_green.gif);
	background-position: left bottom;
	background-repeat: repeat-x;
	line-height: 1em;
	padding-top: .1em;
	margin-bottom: .4em;
}
h1.DottedLine, h2.DottedLine, h3.DottedLine {
	width: 100%;
	display: block !important;
	padding-bottom: .1em !important;
	margin-bottom: .8em !important;
}
.Error, .ErrorMessage, .Required, .field-validation-error {
	color: #F00 !important;
}
.SuccessMessage {
	color: #529900;
	background-color: #DBEBC8;
	padding: .25em;
	font-weight: bold;
}
.ErrorMessage span {
	display: block;
}
.ServiceUnavailableError {
	color: #F00;
	font-style: italic;
	font-size: 1.6em;
}
.Required {
	font-style: italic;
	color: #F00 !important;
}
.Disabled {
	color: #777;
	cursor: text;
}
.Header {
	margin: 0;
}
.Header h1, h1.Header, .Header h2, h2.Header, h3.Header, Header h3 {
	margin: 0;
	display: inline;
	color: #27602E;
}
.AlternateHeader h1, h1.AlternateHeader, .AlternateHeader h2, h2.AlternateHeader, h3.AlternateHeader, .AlternateHeader h3 {
	color: #447733;
}
h1 {
	font-size: 1.6em;
	margin-bottom: .7em;
}
h2 {
	font-size: 1.2em;
	padding-top: .7em;
	padding-bottom: .7em;
}
h3 {
	font-size: 1em;
	padding-bottom: 0;
}
h4 {
	font-size: .8em;
	padding-bottom: .2em;
}
.SubHeader, .SubHeader a, .SubHeader a:active, .SubHeader a:link, .SubHeader a:visited {
	font-size: 1.1em;
	font-weight: bold;
	margin-bottom: 0em;
}
.SecondaryText {
	color: #56862e;
}
.FullWidth {
	width: 100%;
}
.HalfWidth {
	width: 50%;
}
.ThirdWidth {
	width: 33%;
}
input.FullWidth, textarea.FullWidth {
	width: 98%;
}
input.HalfWidth, textarea.HalfWidth {
	width: 49%;
}
input.UrlTextBox {
	width: 31%;
}
.Centered {
	text-align: center;
	margin-left: auto;
	margin-right: auto;
}
.Justify {
	text-align: justify;
}
.AlignLeft {
	text-align: left !important;
}
.AlignRight {
	text-align: right;
}
.AlignTop {
	vertical-align: top;
}
.AlignMiddle {
	vertical-align: middle;
}
.AlignBottom {
	vertical-align: bottom;
}
.AlignCenter {
	text-align: center;
}
.StandardBackground {
	background-color: #fff;
}
.AlternateBackground {
	background-color: #F1FCEE;
}
.AlternateBackgroundDark {
	background-color: #CDF2B3;
}
.ModalBackground {
	background-color: gray;
}
.ModalBackgroundLight {
	background-color: #CCC;
}
table.Grid, .Grid table {
	width: 100%;
	border-width: .1em;
	border-style: solid;
	border-collapse: collapse;
	border: solid .1em #cdcdcd;
}
.Grid td, .Grid th {
	border-width: 0;
	border-bottom-width: .1em;
	border-style: solid;
	text-align: center;
	border: solid .1em #cdcdcd;
	padding: .2em .7em;
	height: 2.2em;
	vertical-align: top;
	border-left: none;
	border-right: none;
}
.Grid th {
	font-weight: bold;
}
.Grid tr {
	border: none;
}
#WikiVersions th {
	background-color: #E2F6CC;
}
.Grid .Header {
	background-color: #E2F6CC;
	margin-bottom: 0em;
}
.ShowDetails .Details, .HideDetails .Summary {
	display: block;
}
.HideDetails .Details, .ShowDetails .Summary {
	display: none;
}
.PageTemplateSelectedLink {
	margin-left: .4em;
	font-weight: bold;
}
.IE7 .StandardMarginLeft, .IE7 .PageTemplateSelectedLink {
	zoom: 100%;
}
.Selected {
	font-weight: bold;
}
.NoItemsMessage {
	font-style: italic;
}
.SortArrow {
	padding: .2em;
	vertical-align: middle;
}
.ArrowSmall {
	background-image: url(http://i2.codeplex.com/Images/v17672/arrow_sm.gif);
	background-position: .2em .5em;
	background-repeat: no-repeat;
	padding-left: 1em;
}
.TopAndBottomBorder {
	border-top: solid 1px #798072;
	border-bottom: solid 1px #798072;
	padding-top: 0.35em;
	padding-bottom: 0.35em;
}
.MoreLink {
	font-weight: bold;
	font-size: .75em;
}
.ContentColumn3 {
	width: 31%;
	min-width: 200px;
}
.CanvasMargin {
	width: 1.4em;
	background-color: #4d4d4d;
	height: 100%;
}
.CanvasBackground {
	background-image: url(http://i2.codeplex.com/Images/v17672/banner_bg_grad.gif);
	background-repeat: repeat-x;
	height: 100%;
}
.CanvasContent {
	background-image: url(http://i2.codeplex.com/Images/v17672/banner_bg.jpg);
	background-repeat: no-repeat;
}
.IE .CanvasBackground {
	height: auto;
}
.IE7 .CanvasContentDiv {
	min-height: 200px;
}
.FullHeightNoIE7 {
	height: 100%;
}
.IE7 .FullHeightNoIE7 {
	height: auto;
}
.SiteHeader {
	padding-right: 1.07em;
}
.SiteHeaderLeft {
	float: left;
	width: 230px;
	height: .88em;
}
.SiteHeaderRight {
	float: right;
	text-align: right;
	min-width: 15.4em;
}
.SiteHeaderProjectRight {
	text-align: right;
	background-image: url(http://i2.codeplex.com/Images/v17672/projectlogobg.png);
	background-repeat: repeat-x;
}
.ProjectLogoLeft {
	background-image: url(http://i2.codeplex.com/Images/v17672/logo_project_left.jpg);
	background-position: right top;
	background-repeat: no-repeat;
	width: 100%;
	min-height: 90px;
}
.IE6 .ProjectLogoLeft {
	height: 90px;
}
.ProjectLogoRight {
	background-image: url(http://i2.codeplex.com/Images/v17672/logo_project_right.jpg);
	background-position: right;
	background-repeat: no-repeat;
	height: 90px;
	width: 148px;
}
.LogoLinkInner {
	visibility: hidden;
}
.LogoLinkOuter, .LogoLinkInner, .ProjectLogoRight .LogoLink {
	width: 134px;
	height: 55px;
}
.LogoLinkOuter {
	padding-top: 20px;
	padding-left: 13px;
}
.VerticalBarTop {
	color: #798072;
	display: inline;
	padding-left: .2em;
	padding-right: .2em;
	font-size: 1em;
	vertical-align: text-top;
	overflow: hidden;
}
.SiteHeaderRightShim {
	width: 15.4em;
	height: 1px;
}
.SiteContent {
	padding-top: 1.07em;
}
.SiteFooter {
	font-size: .9em;
}
.Safari .SiteFooter {
	padding-bottom: 0;
}
.SiteFooter a, .SiteFooter a:active, .SiteFooter a:link, .SiteFooter a:visited {
	color: #3E62A6;
}
.SiteFooter a:hover {
	color: #CE8B10;
}
.NoImages .SiteFooter {
	color: #000;
}
.SiteContentTable {
	margin-left: auto;
	margin-right: auto;
}
.SiteContentTopPadding {
	padding-top: 2em;
}
.SidebarContainer {
	width: 22em;
}
.IE6 .SidebarContainer {
	margin-top: 1.5em;
}
.ProjectContent .SideBar {
	display: block;
	margin-top: 1.3em;
	margin-left: .7em;
}
.ProjectContent .SideBar .ContentPanel .TopBorder, .ProjectContent .SideBar .ContentPanel .RoundedContent {
	background-image: url('http://i2.codeplex.com/Images/v17672/sidebarheaderbg.gif');
	background-repeat: repeat-x;
}
.SideBar {
	width: 15.1em;
	min-width: 15.1em;
}
.SideBarPadding, .SideBarPadding div {
	width: 17px;
}
.RightSideBar {
	margin-top: 2em;
	padding: 0 0 0.2em 0;
	width: 24.25em;
}
.OuterBox {
	padding: 0 0 0.2em 0;
	width: 40em;
	margin: auto;
}
.OuterBoxNoWidth {
	padding: 0 0 0.2em 0;
	margin: auto;
}
.GradientSideBar, .GradientBox {
	border: solid 0.1em #ccc;
	background: transparent url(http://i2.codeplex.com/Images/v17672/sidebox.png) repeat-x;
}
.SideBarControl, .InnerBox, .SideBarHeader {
	background-color: #fff;
	clear: both;
	border: solid 0.1em #ccc;
	margin: 0.3em 0.3em 0.1em 0.3em;
	padding: .5em;
}
.SideBarControlNoPadding {
	padding: 0em;
}
.SideBarHeader {
	background-color: transparent;
	border: none;
	padding: 0 .8em 0 .8em;
}
.SideBarHeader h2 {
	padding: 0;
	margin: 0;
	color: #798072 !important;
}
.SideBarHeader h2 span {
	font-size: .85em;
	font-weight: normal;
}
.SideBarControl h2 {
	color: #000;
}
.InnerBox h2 {
	color: #30332d;
	font-weight: bold;
	padding: 0;
	margin: 0;
}
.MainContent {
	width: 35em;
}
.MainContentMinWidthDiv {
	overflow: hidden;
	height: 1px;
	width: 17.1em;
}
.MainContentMinWidthContent {
	margin-top: -1px;
}
.FlashMessage {
	background-color: #3E62A6;
	color: White;
	font-weight: bold;
	padding: .5em;
	margin-bottom: 1em;
	text-align: center;
	-khtml-border-radius: .5em;
	-moz-border-radius: .5em;
	-o-border-radius: .5em;
	border-radius: .5em;
}
.SecondarySearchButton {
	padding: 0;
	margin-left: -0.55em;
	height: 21px;
	width: 21px;
	vertical-align: middle;
}
span.ActivityArrow {
	color: #30332D;
	font-weight: bold;
	background: #CCCCC2 url('http://i2.codeplex.com/Images/v17672/flag.gif') no-repeat 0 0.2em;
	background-position: right;
	padding: .1em .8em .1em .3em;
}
.IE6 iframe.FullPanel {
	filter: alpha(opacity=0);
}
.FullPanel {
	width: 100%;
	height: 100%;
	overflow: hidden;
}
.IE6 .UpdateProgressPanel .FullPanel {
	position: absolute;
}
.IE table.FullPanel {
	table-layout: fixed;
}
.ModalBackground {
	background-color: gray;
	filter: alpha(opacity=70);
	opacity: 0.7;
}
.UpdateProgressPanel, .UpdateProgressPanel .ModalBackground, .UpdateProgressPanel #UpdateProgressTable {
	position: absolute;
	top: 0;
	left: 0;
}
.UpdateProgress {
	background-color: #DAF1B2;
	color: #008000;
	font-weight: bold;
	border: solid .1em black;
	width: 13.2em;
	height: 4.1em;
	overflow: hidden;
	text-align: center;
	position: absolute;
	z-index: 1000000;
	visibility: hidden;
}
.UpdateProgressText {
	position: relative;
	top: 1.44em;
}
.ui-dialog {
	background-color: white;
	border: solid 1px #cfcfcf;
	position: absolute;
	overflow: hidden;
}
.ui-dialog .ui-dialog-titlebar, .ui-dialog .ui-dialog-titlebar-close {
	display: none;
}
.ui-dialog-overlay {
	background-color: Black;
}
.ui-dialog .ui-dialog-content {
	margin: 1.2em;
}
.ui-widget-overlay {
	opacity: 0.5;
	filter: alpha(opacity=50);
	-moz-opacity: 0.5;
	background: #000000;
	position: absolute;
	top: 0;
	left: 0;
}
.ClosePanel, .ChangePanel, .LicensePanel, .PullRequestPanel {
	background-color: White;
	border: solid 1px #333;
	padding-bottom: 1em;
}
.ClosePanel .ui-dialog-titlebar, .ChangePanel .ui-dialog-titlebar, .LicensePanel .ui-dialog-titlebar, .PullRequestPanel .ui-dialog-titlebar {
	background-color: #e6e6e6;
	border-bottom: solid 1px #333;
	font-weight: bold;
	padding: 0.25em 0.5em;
	display: block;
}
.ClosePanel .Body, .ChangePanel .Body, .LicensePanel .Body, .PullRequestPanel .Body {
	padding: 0.5em;
}
.ClosePanel .Buttons, .ChangePanel .Buttons, .ChangePanel .Footer, .LicensePanel .Buttons, .PullRequestPanel .Buttons {
	text-align: right;
	margin-top: 0.25em;
	margin-right: .5em;
}
.ClosePanel {
	width: 30em;
}
.ClosePanel .Body .CommentBox {
	font-size: inherit;
	width: 98%;
}
.ChangePanel {
	width: 20em;
}
.ChangePanel .Body .Content {
	border: solid 1px #aaa;
	height: 10em;
	overflow: auto;
	width: 100%;
}
.IE6 .ChangePanel .Body .Content {
	width: auto;
}
.ChangePanel .Body .DynamicContent {
}
.ChangePanel .Footer {
	text-align: center;
}
.ChangePanel .PanelErrorLabel {
	color: Red;
	padding: 0px 10px;
	visibility: hidden;
	text-align: center;
}
.LicensePanel {
	width: 45em;
}
.LicensePanel .Body .CommentBox {
	font-size: inherit;
	width: 98%;
}
ul.ContextMenu {
	background-color: #fff;
	border: solid 1px #888;
	color: #333;
	list-style-type: none;
	list-style-image: none;
	margin: 0;
	padding: 0;
	position: absolute;
}
ul.ContextMenu li {
	cursor: pointer;
	margin: .2em .2em .2em .2em;
	padding: .1em .5em;
}
ul.ContextMenu li:hover {
	background-color: #888;
	color: #fff;
}
ul.ContextMenu li.GroupCaption {
	color: #333;
	margin-top: .5em;
}
ul.ContextMenu li.GroupCaption:hover {
	background-color: #fff;
	color: #333;
}
ul.ContextMenu li.GroupMember {
	padding-left: 1em;
}
ul.ContextMenu li.Nested {
	background-image: url('http://i2.codeplex.com/Images/v17672/nestedMenu.gif');
	background-position: right 0px;
	background-repeat: no-repeat;
}
ul.ContextMenu li.Nested:hover {
	background-image: url('http://i2.codeplex.com/Images/v17672/nestedMenu.gif');
	background-position: right -16px;
	background-repeat: no-repeat;
}
div.EmailOptIn .AdditionalText {
	color: Gray;
}
.ui-datepicker-prev, .ui-datepicker-prev a, .ui-datepicker-next, .ui-datepicker-next a {
	font-family: Tahmoa;
	font-size: 8.3pt;
	color: Black;
	text-decoration: none;
	cursor: pointer;
}
.ui-datepicker-prev {
	float: left;
	padding-left: .25em;
	padding-top: .25em;
}
.ui-datepicker-next {
	float: right;
	margin-right: .25em;
	padding-top: .25em;
}
.ui-datepicker-header {
	font-family: Tahmoa;
	font-size: 8.3pt;
	font-weight: 700;
	text-align: center;
	border: solid 1px #646464;
	border-bottom: none;
	background-color: White;
}
.ui-datepicker {
	background-color: White;
	border: solid 1px #646464;
	border-top: none;
	font-family: Tahoma;
	font-size: 8.3pt;
	color: Black;
	width: 14em;
}
.Chrome .ui-datepicker {
	background-color: White;
	border: solid 1px #646464;
	border-top: none;
	font-family: Tahoma;
	font-size: 8.3pt;
	color: Black;
	width: 14em;
	margin-left: -0.75em !important;
}
.Safari .ui-datepicker {
	background-color: White;
	border: solid 1px #646464;
	border-top: none;
	font-family: Tahoma;
	font-size: 8.3pt;
	color: Black;
	width: 14em;
	margin-left: -0.75em !important;
}
.ui-datepicker a {
	color: Black;
	text-decoration: none;
}
.ui-datepicker-title-row {
	padding: .35em;
}
.ui-datepicker-days-cell {
	padding: .35em;
	text-align: center;
}
.ui-datepicker-days-cell-over {
	background-color: #edf9ff;
	color: #0066cc;
	cursor: pointer;
}
.ui-datepicker-current-day {
	background-color: #edf9ff;
	border: solid 1px #daf2fc;
	color: #0066cc;
}
.ui-datepicker-control, .ui-datepicker-current {
	display: none;
}
.ui-datepicker-trigger {
	margin-left: .5em;
	cursor: pointer;
}
.ui-datepicker-cover {
	display: none;
	display: block;
	position: absolute;
	z-index: -1;
	filter: mask();
	top: -4px;
	left: -4px;
	width: 193px;
	height: 200px;
}
.ui-helper-hidden {
	display: none;
}
.ui-helper-hidden-accessible {
	position: absolute;
	left: -99999999px;
}
.ui-helper-clearfix:after {
	content: \"+".\""+@";
	display: block;
	height: 0;
	clear: both;
	visibility: hidden;
}
.ui-helper-clearfix {
	display: inline-block;
}
* html .ui-helper-clearfix {
	height: 1%;
}
.ui-helper-clearfix {
	display: block;
}
.AltPost .discussionListContent PRE {
	background-color: #EFF0F3;
}
.discussionListContent UL {
	list-style-image: none;
	list-style-type: disc;
}
.discussionListContent UL LI UL {
	list-style-type: circle;
}
.discussionListContent UL LI UL LI UL {
	list-style-type: square;
}
.discussionListContent LI {
	MARGIN-TOP: 0.3em;
	MARGIN-BOTTOM: 0.3em;
	VERTICAL-ALIGN: middle;
}
.ModalDialogHeaderIcon {
	float: left;
}
.ModalDialogHeaderTitle {
	font-weight: bold;
}
.ModalDialogHeaderText {
	text-align: left;
	padding-top: 5px;
}
.ModalDialogContent {
	margin-left: 25px;
	margin-top: 1.5em;
}
.ModalDialogContentButtons {
	margin-top: 1.5em;
}
.ModalDialogContentButtons A {
	margin-left: 6px;
}
.EmailSubscriptionText {
	color: Gray;
}
.EmailSubscriptionCheck {
	padding-left: 18px;
}
.EmailSubscriptionRadioButtonAlignment {
	margin-left: -.5em;
}
.tooltip {
	font-family: " + "\"Segoe UI\",\"Microsoft Sans Serif" + @",Arial,Geneva,Sans-Serif;
	line-height: 1.5em;
	color: #333333;
	font-size: 11px;
	background-color: #FFFFFF;
	border: 4px solid #408cb3;
	padding-bottom: 1em;
}
.tooltip p {
	margin: 0 0 8px 0;
}
.CodePlexPageHeader {
	color: #6d8d34;
	font-size: 1.6em;
	font-weight: bold;
	background-repeat: no-repeat;
	vertical-align: middle;
}
#adzerk p {
	margin-left: 130px;
	padding-bottom: .5em;
}
#adzerk a {
	text-decoration: none;
}
#adzerk1 p {
	font-size: .95em;
	margin-top: 0;
	margin-bottom: 0;
}
#adzerk_by {
	margin-bottom: 0;
}
#adzerk_by a {
	font-family: " + "\"Segoe UI\",\"Microsoft Sans Serif" + @",Arial,Geneva,Sans-Serif;
	font-weight: bold;
}
.FileListItemDiv {
	clear: left;
	width: 100%;
	margin-bottom: .8em;
}
.SigninMessage {
	background-color: #FFF0CC;
	border: solid 1px #FFC536;
	padding: .5em;
	margin-bottom: 1em;
	width: 95%;
}
.SignInOption {
	border-top: solid 1px #e6e6e6;
	padding-top: .5em;
	width: 99%;
}
.LoungeAdsBottomLinks {
	font-size: 0.8em;
	text-align: center;
	margin-top: .5em;
	margin-bottom: .5em;
	padding-bottom: .625em;
}
a#subscriptionChange {
	vertical-align: top;
}
table#MetaDataTable td {
	padding-right: 10px;
}
table#MetaDataTable td#ForkActionsCell {
	border-left: 1px solid #cdcdcd;
	margin-left: 4px;
	padding-left: 10px;
	padding-top: 4px;
}
.CurrentReleaseBackground {
	background: #fff;
	background-image: url(http://i2.codeplex.com/Images/v17672/dl_back.jpg);
	background-repeat: no-repeat;
	background-position: right top;
}
.CurrentReleaseDetail {
	font-size: x-small;
}
.YourProfileLink a, .YourProfileLink a:link, .YourProfileLink a:active, .YourProfileLink a:visited {
	color: #3E62A6!important;
	text-decoration: underline !important;
}
.YourProfileLink a:hover {
	color: #CE8B10!important;
	text-decoration: underline !important;
}
.EnhancedTextBoxTable {
	position: relative;
	top: -1em;
}
.Safari .EnhancedTextBoxFix, .Opera .EnhancedTextBoxFix {
	padding-left: 1em;
}
.CharCounter {
	text-align: right;
	color: #666;
	font-size: .9em;
	padding-right: 1em;
}
.CountdownControl {
	border: solid 0.1em #ccc;
	padding: .3em;
	xmax-width: 40em;
	xposition: relative;
	margin: 0 0 .7em 0;
}
.IE6 .CountdownControl {
	position: relative;
}
.ProjectSearchTextBox {
	padding-left: .3em;
	width: 12.3em;
	height: 1.4em;
	color: #000 !important;
}
.ProjectSearchButton {
	padding: 0;
	vertical-align: middle;
	margin-left: -.6em;
	cursor: pointer;
}
.Chrome .ProjectSearchButton {
	margin-top: .1em;
}
.SiteHeaderProjectRight .ProjectSearchTextBox {
	width: 15.5em;
}
.IE6 .ProjectTitleControl {
	position: relative;
}
.ProjectTitleControl, .ProjectTitleControl a, .ProjectTitleControl a:link, .ProjectTitleControl a:visited {
	color: #000;
	text-decoration: none;
}
.SearchWikiControl {
	display: inline;
}
.WikiSearchTextBox {
	width: 15.5em;
}
.WikiSearchButton {
	padding-left: 0.3em;
}
.PageIndexControl {
	font-weight: normal;
}
.PageIndexControl .Range, .PageIndexControl .Count, .PageIndexControl .Selected {
	font-weight: bold;
}
.PageSizeControl {
	font-weight: normal;
}
.PageSizeControl .Selected {
	font-weight: bold;
}
.ContentPanel {
	border-width: .1em;
}
.ContentPanel .Header {
}
.ContentPanel .RoundedContent .HeaderPanel {
	padding: .2em .7em;
	background-image: none;
	background-color: Transparent;
}
.ContentPanel .RoundedContent .FooterPanel {
	padding: .2em .7em;
	background-image: none;
	background-color: Transparent;
}
.IE6 .ContentPanel .RoundedContent .HeaderPanel, .IE6 .ContentPanel .RoundedContent .FooterPanel {
	margin-top: 0 !important;
	margin-bottom: 0 !important;
}
.ContentPanel .RoundedContent {
	background-image: url(http://i2.codeplex.com/Images/v17672/contentpanel_gradientbg.gif);
	background-repeat: repeat-x;
	background-color: #fff;
	border-color: #e1e1e1;
	border-color: #a0a0a0;
	min-height: 20px;
	border-style: solid;
	border-width: 0;
	width: auto;
}
.FF .ContentPanel .RoundedContent, .Safari .ContentPanel .RoundedContent, .Opera .ContentPanel .RoundedContent {
	overflow: visible !important;
}
.ContentPanel .RoundedCornerContainer {
	width: auto;
}
.ContentPanel .TopBorder {
	background-image: url(http://i2.codeplex.com/Images/v17672/contentpanel_gradientbg.gif);
	background-repeat: repeat-x;
}
.ContentPanel .OuterBorder {
	border-width: 0;
	margin: 0;
	overflow: hidden;
	border-color: #e1e1e1;
	border-color: #a0a0a0;
	border-style: solid;
}
.ContentPanel .RoundedBorder {
	margin: 0em;
	border-width: 0;
	overflow: hidden;
	height: 1px;
	background-color: #fff;
	border-color: #e1e1e1;
	border-color: #a0a0a0;
	border-style: solid;
	width: auto;
}
.ClearPanel .HeaderPanel, .ClearPanel .RoundedBorder, .ClearPanel .RoundedContent {
	background-image: none;
	background-color: White;
}
.SignInPanel .RoundedContent, .SignInPanel .RoundedBorder {
	background-color: #46ae28;
	background-image: none;
}
.FF .SignInPanel td a {
	line-height: 1.6em;
}
.AlternateBackgroundPanel {
	background-image: none;
	background-color: #F1FCEE;
	border: 1px solid black;
}
.TabStripNew table {
	border-bottom: 2px solid #317200;
}
.TabStripNew table td {
	white-space: nowrap;
	vertical-align: bottom;
}
.InactiveTabNew {
	border-right: 1px solid #E7E7E3;
	border-left: 1px solid #E7E7E3;
}
.InactiveTabNew, .LastTabNew {
	background-position: top;
	background-image: url(http://i2.codeplex.com/Images/v17672/projecttab_bar.png);
	background-repeat: repeat-x;
}
.ActiveTabNew {
	background-color: #317200;
	background-image: url(http://i2.codeplex.com/Images/v17672/projecttab_live.png);
	background-repeat: repeat-x;
}
.TabLinkNew {
	text-decoration: none !important;
	white-space: nowrap;
	display: block;
	cursor: pointer;
	width: 100%;
	height: 100%;
	padding: 6px 0;
	text-align: center;
}
.InactiveTabNew, .InactiveTabNew a, .InactiveTabNew a:link, .InactiveTabNew a:visited {
	color: #30332d;
}
.ActiveTabNew, .ActiveTabNew a, .ActiveTabNew a:link, .ActiveTabNew a:visited {
	color: #FFF;
	font-weight: bold;
}
.TabPadding {
	margin-left: 1.75em;
	margin-right: 1.75em;
}
.LastTabNew {
	padding: 4px;
	text-align: right;
}
#SubTabs {
	margin-top: .25em;
	padding-left: .9em;
	height: 100%;
}
#SubTabs .LeftNav {
	float: left;
}
#SubTabs .RightNav {
	float: right;
}
.Safari .EditPreviewPost .ContentPanel .RoundedContent {
	overflow: hidden !important;
}
.TagCloud-Lowest {
	font-size: 1.133em;
	font-weight: 100;
}
.TagCloud-Low {
	font-size: 1.275em;
	font-weight: 300;
}
.TagCloud-Medium {
	font-size: 1.42em;
	font-weight: 500;
}
.TagCloud-High {
	font-size: 1.63em;
	font-weight: 700;
}
.TagCloud-Highest {
	font-size: 2.125em;
	font-weight: 900;
}
.AutoCompletePanel {
	font-size: 95%;
	border: solid .1em #999;
	background-color: #f0f0f0;
	padding: .15em;
}
.AutoCompletePanel div.Row {
	color: #000;
	cursor: pointer;
	background-color: transparent;
	padding: .15em .25em;
}
.AutoCompletePanel div.Selected {
	color: #fff;
	background-color: #aaa;
}
.VoteBox {
	float: left;
	width: 4.25em;
}
div.votebox {
	border: solid .1em #BBB;
	text-align: center;
	width: 3.75em;
}
div.votebox .top, div.votebox .topClosed {
	background-image: url(http://i2.codeplex.com/Images/v17672/votebox_bkg.gif);
	background-repeat: repeat-x;
	color: #333;
	height: 2.19em;
}
div.votebox .topClosed {
	background-image: url(http://i2.codeplex.com/Images/v17672/votebox_closed_bkg.gif);
}
div.votebox .count {
	display: block;
	font-size: 1.0em;
	font-weight: bold;
	margin-bottom: -.4em;
}
div.votebox .bottom {
	background: #FFF;
	border-top: solid .1em #BBB;
	color: #BBB;
	padding-bottom: .1em;
}
.VoteBox .SubLink {
	font-size: .8em;
}
.InfoBox {
	border: solid .1em #bbb;
	text-align: center;
	width: 4.5em;
}
.InfoBox .Top {
	background-image: url(http://i2.codeplex.com/Images/v17672/votebox_bkg.gif);
	background-repeat: repeat-x;
	color: #333;
	min-height: 3.5em;
	vertical-align: top;
}
.InfoBox .Top .Title {
	font-size: .7em;
}
.InfoBox .Top .Date, .InfoBox .Top .Count {
	margin-top: -.1em;
	line-height: 1.1em;
	font-size: 1.2em;
	font-weight: bolder;
}
.InfoBox .Top .Time, .InfoBox .Top .Caption {
	font-size: .8em;
	padding-top: .4em;
}
.InfoBox .Bottom {
	background-color: #fff;
	border-top: solid .1em #bbb;
	color: #686868;
	font-size: .8em;
	height: 1.5em;
	padding: .1em 0 .1em 0;
}
#ProjectRelease .InfoBox .Top {
	background-image: url(http://i2.codeplex.com/Images/v17672/votebox_closed_bkg.gif);
	height: 5em !important;
}
.RemoveUserCell {
	width: 6.25em;
	text-align: center;
}
.UserNameCell {
	width: 15.63em;
	text-align: left;
}
.RequestAccessUserCell {
	width: 22.5em;
	text-align: left;
}
.UserRoleCell {
	width: 20em;
	text-align: left;
}
.EditPreviewPost {
	margin-bottom: .25em;
}
.EditPreviewPost .FormattingGuide {
	width: 15em;
	vertical-align: top;
	padding-left: .5em;
	font-size: 0.8em;
}
.EditPreviewPost .EditInfo {
	text-align: right;
}
.EditPreviewPost .EditCommands {
	text-align: left;
}
.EditPreviewPost .PreviewArea {
	min-height: 22.3em;
}
.FF .EditPreviewPost .PreviewArea table {
	margin-top: 1px;
	margin-left: 1px;
}
.EditPreviewPost .MarkupGuide {
	float: right;
	text-align: left;
	width: 14em;
}
div#ConfigureView {
	float: right;
	font-size: .9em;
	width: 19em;
}
div#ConfigureView .Content {
	padding: .75em;
}
div#ConfigureView .Left {
	margin-right: 11em;
	text-align: left;
}
div#ConfigureView .Right {
	float: right;
	text-align: left;
	width: 12em;
}
div#ConfigureView .Clear {
	clear: both;
	margin-bottom: 1.5em;
}
div#ConfigureView .Filter {
	vertical-align: middle;
	width: 100%;
}
div#ConfigureView .Sort {
	vertical-align: middle;
	width: 100%;
}
div#ConfigureView .Search {
	margin-right: .75em;
	vertical-align: middle;
	width: 7em;
}
div#ConfigureView a.selected {
	font-weight: bold;
	text-decoration: none;
	color: Black;
}
.RssFeedsPanel {
	margin-top: 0;
	margin-bottom: 0;
	padding-top: 0;
	padding-bottom: 0;
	cursor: pointer;
	text-align: left;
	z-index: 999999;
	list-style-image: none;
	list-style-type: none;
	padding: 0 0 0 0;
	white-space: normal;
}
.IE6 ul.RssFeedsPanel li {
	width: 10em;
}
abbr.smartDate {
	border: 0;
}
.CodePlexPageTemplate, .CodePlexPageSizeTemplate {
	font: normal small arial,verdana;
}
.CodePlexPageSizeTemplate {
	float: right;
	text-align: right;
}
.PageTemplateItemsInfoText {
	margin-left: .32em;
}
.PageTemplateSeperator {
	margin-left: .32em;
}
.PageTemplateSelectedLink {
	margin-left: .32em;
	font-weight: bold;
}
.WorkItemField {
	width: 15%;
}
.WorkItemType {
	font-weight: bold;
	color: Red;
}
.RatingStarContainer {
	display: inline;
}
.RatingStar {
	width: 12px;
	height: 12px;
	padding: 0 9px 0 0;
	background-repeat: no-repeat;
	background-position: center;
}
.IE .RatingStar {
	padding: 0 12px 0 0;
}
.EditStarMode .RatingStar {
	cursor: pointer;
}
.FilledRatingStar {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_gold_full.png);
}
.FilledRatingStarUser {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_blue_full.png);
}
.FilledRatingStarMuted {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_gold2_full.png);
}
.EmptyRatingStar {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_empty.png);
}
.HalfRatingStar {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_gold_half.png);
}
.HalfRatingStarUser {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_blue_half.png);
}
.HalfRatingStarMuted {
	background-image: url(http://i2.codeplex.com/Images/v17672/star_gold2_half.png);
}
.ProjectSignInControl a, .ProjectSignInControl a:link, .ProjectSignInControl a:visited, .ProjectSignInControl a:hover, .ProjectSignInControl a:active {
	color: #fff;
	font-size: .9em;
}
.ProjectSignInControl a:hover {
	color: #ccc;
	font-size: .9em;
}
.ProjectSignInControl a:active {
	color: #fff;
	font-size: .9em;
}
.NoImages .ProjectSignInControl a, .NoImages .ProjectSignInControl a:active, .NoImages .ProjectSignInControl a:link, .NoImages .ProjectSignInControl a:visited {
	color: #3E62A6;
}
.NoImages .ProjectSignInControl a:hover {
	color: #CE8B10;
}
.ProjectEditProjectDetails {
	margin-top: .9em;
}
.popupWindow {
	border: solid 2px #444444;
}
.popupWindow .description {
	margin-top: 1em;
	padding-top: 1em;
	border-top: solid 1px #CFCFCF;
}
.popupWindow .ui-dialog-titlebar, .popupWindow .ui-dialog-titlebar-close {
	display: block;
}
.popupWindow .ui-widget-header {
	background-color: #EAEAE6;
	font-weight: bold;
}
.popupWindow .ui-dialog-titlebar {
	padding: 0.2em 0.3em 0.2em 0.3em;
	position: relative;
}
.popupWindow .ui-dialog-titlebar-close {
	height: 18px;
	margin: -10px 0 0;
	padding: 1px;
	position: absolute;
	right: 0.3em;
	top: 50%;
	width: 19px;
}
.popupWindow .ui-widget-header .ui-icon {
	background-image: url('http://i2.codeplex.com/Images/v17672/btn_close-off.png');
	height: 17px;
	width: 17px;
	background-repeat: no-repeat;
	text-indent: -99999px;
	display: block;
	cursor: pointer;
}
.popupWindow .ui-widget-header .ui-state-hover .ui-icon {
	background-image: url('http://i2.codeplex.com/Images/v17672/btn_close-on.png');
}
#SearchFlyoutContainer li {
	list-style-type: none;
	list-style-image: none;
}
.PageStep {
	background-repeat: no-repeat;
	display: inline-block;
	padding-left: 20px;
	margin-right: .5em;
}
.Safari .PageStep {
	min-height: 2em;
	padding-top: .3em;
}
.StepOneActive {
	background-image: url(http://i2.codeplex.com/Images/v17672/step1_active.gif);
	font-weight: bold;
	color: #27602E;
}
.StepTwoActive {
	background-image: url(http://i2.codeplex.com/Images/v17672/step2_active.gif);
	font-weight: bold;
	color: #27602E;
}
.StepTwoInactive {
	background-image: url(http://i2.codeplex.com/Images/v17672/step2_inactive.gif);
}
.StepThreeActive {
	background-image: url(http://i2.codeplex.com/Images/v17672/step3_active.gif);
	font-weight: bold;
	color: #27602E;
}
.StepThreeInactive {
	background-image: url(http://i2.codeplex.com/Images/v17672/step3_inactive.gif);
}
.StepComplete {
	background-image: url(http://i2.codeplex.com/Images/v17672/step_checked.gif);
}
#SourceControlSurvey {
	margin-left: 1.2em;
}
#SourceControlSurvey .SubText {
	margin-left: .3em;
}
#donateAdsContainer, #donateAdsSecondLine, .instrumentationInfo {
	margin-left: 1.6em;
}
.instrumentationInfo input[type=" + "\"text\"" + @"] {
	width: 25em;
}
#PrimaryLabel {
	font-family: " + "\"Segoe UI\",\"Microsoft Sans Serif" + @",Arial,Geneva,Sans-Serif;
	font-size: 1.2em;
	font-weight: bold;
}
.SearchButton {
	background: url(http://i2.codeplex.com/Images/v17672/search-button-off.gif) no-repeat 0 0;
	width: 60px;
	border: 0;
	margin: 1px 0 0 -4px;
	cursor: default;
}
.FF .SearchButton, .FF .SearchButtonOn {
	padding-top: 2px;
}
.Chrome .SearchButton, .Chrome .SearchButtonOn, .Safari .SearchButton, .Safari .SearchButtonOn {
	padding-top: 3px;
}
.SearchButtonOn {
	background: url(/images/v0/search-button-on.gif) no-repeat 0 0;
	width: 60px;
	border: 0;
	margin: 1px 0 0 -4px !important;
	cursor: pointer;
}
.Footnote {
	color: #529900;
	font-size: 0.9em;
}
.FootnoteMarker {
	color: #529900;
	vertical-align: text-top;
}
#SearchResults .CurrentReleaseRating {
	margin-top: 0.5em;
}
#tabnav {
	background-image: url('http://i2.codeplex.com/Images/v17672/bkgd_tabnavbottom.gif');
	background-repeat: repeat-x;
	margin: 0;
	padding: 0;
}
.NotPublishedHeader {
	color: #6d8d34;
	font-size: 1.6em;
	font-weight: bold;
	background-repeat: no-repeat;
	vertical-align: middle;
}
.NotPublishedWarning {
	color: #F00;
	font-size: 1.8em;
	font-weight: bold;
}
.ProjectPageContent {
	border-style: solid;
	border-width: .1em;
	border-color: #B8C1CA;
	border-top: none;
}
.editCell {
	width: 7.5em;
	text-align: right;
}
.ProjectDirectory {
	width: 100%;
}
.ProjectDirectory th {
	padding: .25em;
	background-color: #DDD;
	border-right: solid .1em #BBB;
	border-left: solid .1em #BBB;
	text-align: left;
	font-weight: normal;
}
.ProjectDirectory td {
	padding: .25em;
}
.ProjectDetailsDescription {
	display: block;
}
.ProjectDetailsRecentActivity {
	text-decoration: underline;
	display: block;
}
.ProjectDetailsText {
	display: block;
}
.ProjectSearch {
	border: #BBB .1em solid;
	margin-bottom: .63em;
}
.ProjectSearchContent {
	padding: .2em .7em .2em .7em;
}
.ProjectSearchDropDown {
	float: right;
	margin-left: 63em;
}
.ProjectExpandedItem {
	color: #000;
	font-size: .9em;
	font-weight: bold;
	padding-top: .4em;
}
.ProjectDirectoryHeader {
	background-image: url('http://i2.codeplex.com/Images/v17672/projectbox_bkg.gif');
	background-repeat: repeat-x;
	color: #000;
	font-size: 0.9em;
	font-weight: bold;
	height: 1.25em;
	padding: .2em .7em .2em .7em;
}
.ProjectDirectoryHeaderControl {
	height: 1.8em;
	font-size: 80%;
	font-weight: normal;
}
.SearchResultText {
	font-weight: bold;
	font-style: italic;
	font-size: 1.3em;
}
.SimilarProjectTags {
	color: #777;
	font-size: 1em;
}
.SimilarProjectTagsBold {
	font-weight: bold;
	font-size: 1.1em;
	color: #444;
}
.ProjectDirectoryListItemSeparator {
	height: 3px;
	border-collapse: collapse;
	margin-top: 0;
	margin-bottom: 0;
	padding-top: 0;
	padding-bottom: 0;
}
.ProjectDirectoryListItemSeparator td {
	height: 3px;
	border: 0;
	margin-top: 0;
	margin-bottom: 0;
	padding-top: 0;
	padding-bottom: 0;
}
.ProjectDirectoryListItemSeparator hr {
	color: #ccc;
	border: 0;
	border-bottom: 1px solid #ccc;
	margin-top: 0;
	margin-bottom: 0;
}
.ProjectListSubLink a, .ProjectListSubLink a:link, .ProjectListSubLink a:visited, .ProjectListSubLink a:active {
	text-decoration: none;
}
.ProjectListSubLink a:hover {
	text-decoration: underline;
}
.SearchTextBox {
	padding-left: .3em;
	width: 34em;
	font-size: 0.9em;
	vertical-align: bottom;
}
.ResultListPanel .CurrentReleaseColumn {
	width: 14em;
}
.highlightterm {
	background-color: yellow;
}
.SearchArea {
	padding-left: 5em;
	padding-right: .5em;
}
.RefinementPane {
	width: 14.5em;
	margin-top: 1.20em;
	padding-left: 1em;
}
.RefinementPane ul {
	margin-top: 0;
}
.RefinementPane ul.Filter {
	list-style-image: none;
	list-style-type: none;
	margin-left: -1.5em;
}
.RefinementPane a {
	text-decoration: none;
}
.RefinementPane h2 {
	margin: 0;
	padding: 0.3em;
	background-color: #A1A69C;
	color: #fff;
}
.RefinementPane h3 {
	color: #000;
	font-weight: bold;
	padding: 0 0 0 0.3em;
	margin: 0.5em 0 0.5em 0;
}
.FF .RefinmentAdvSearchPadding, .Safari .RefinmentAdvSearchPadding {
	margin-top: 2.75em;
}
.FF .ResultsAdvSearchPadding, .Safari .ResultsAdvSearchPadding {
	margin-top: 3em;
}
.SearchResults {
	margin-right: 1em;
	margin-top: 1em;
}
#advancedSearch {
	margin-top: .5em;
	padding-right: 3em;
	width: 34.5em;
	margin-left: 4em;
}
.IE #advancedSearch {
	padding-right: 0;
	width: 35em;
}
#licenseSelection ul {
	list-style-image: none;
	list-style-type: none;
	margin-left: -1.5em;
}
.advancedLeftColumn {
	float: left;
	text-align: right;
	width: 16em;
}
.advancedRightColumn {
	padding-left: .5em;
	float: left;
	width: 18em;
}
#advancedDevelopmentStatus {
	width: 18.25em;
}
.FF #advancedDevelopmentStatus {
	width: 18em;
}
.AgreementPanel {
	border: 1px solid #888888;
	height: 12em;
	width: 99%;
}
.Opera .AgreementPanel {
	overflow: auto;
}
.ProjectCreationSubtext {
	color: #777;
	font-size: .8em;
}
.ProjectCreationPanelPadding {
	padding: .2em 1.0em .5em 1.0em;
}
.ManageCoordinatorTopic td.TopicOperations {
	font-size: 85%;
	white-space: nowrap;
	padding-right: .4em;
}
.LicenseEditHeader {
	background-color: #eee;
	padding: 0.5em 1em;
	font-size: 85%;
}
.LicenseEditHeader td {
	padding: .2em .7em;
}
.LicenseEditHeader td td {
	padding: 0.1em 0.75em 0.1em 0;
}
.LicenseEditorBody {
	padding: 1em;
}
#LicenseHistorySideBar {
	margin: 1.3em auto auto 0.7em;
}
#LicenseHistorySideBar .GradientBox {
	width: 300px;
}
.ReadOnlyLink {
	color: #dcdcdc;
}
#internalRequest {
	color: Red;
	margin-top: 1em;
	border-top: solid 1px #ccc;
	border-bottom: solid 1px #ccc;
}
.ProjectRolesTable {
	border-collapse: collapse;
}
.Opera .ProjectRolesTable {
	border-collapse: inherit;
	border-right: solid .1em #BBB;
	border-bottom: solid .1em #BBB;
}
.ProjectRolesTable td, .ProjectRolesTable th {
	border: solid .1em #BBB;
	padding: .15em .5em .15em .5em;
	white-space: nowrap;
}
.ProjectRolesTable th {
	background-color: #EEE;
}
.ProjectRolesTable tr.ThickBorder td, .ProjectRolesTable tr.ThickBorder th {
	border-top: solid .13em #999;
}
.ProjectRolesTable .Permission {
	color: Green;
	font-size: 65%;
	font-weight: bold;
	text-align: center;
}
#statsLink {
	cursor: pointer;
}
#statsLink:hover {
	text-decoration: none;
}
#statsLink .Left {
	width: 20px;
	height: 20px;
	background-image: url(http://i2.codeplex.com/Images/v17672/view_stats_arrow.png);
	background-repeat: no-repeat;
	float: left;
}
#statsLink .Middle {
	width: 246px;
	height: 20px;
	background-image: url(http://i2.codeplex.com/Images/v17672/view_stats_background.png);
	background-repeat: repeat-x;
	float: left;
	color: White;
	text-align: center;
}
.FF #statsLink .Middle {
	width: 246px;
}
#statsLink .Right {
	width: 20px;
	height: 20px;
	background-image: url(http://i2.codeplex.com/Images/v17672/view_stats_right.png);
	background-repeat: no-repeat;
	float: left;
}
#manageOpenings .Links {
	text-align: left;
	padding-bottom: 1em;
}
#manageOpenings .Header {
	margin-bottom: 1em;
}
#manageOpenings .ExampleColumn {
	float: right;
	width: 20.7em;
	margin-top: 0;
	border: solid 1px #CECECE;
	padding: .5em;
}
#manageOpenings .ManageFormColumn {
	float: left;
	width: 69%;
}
#editProjectOpening {
	margin-top: 1em;
}
#openingDescriptionContainer, #editOpeningTags {
	margin-left: 1.8em;
}
#openingDescriptionContainer textarea {
	width: 40em;
}
#jobDescription .wikidoc {
	overflow: hidden;
	width: 18.25em;
}
#joinUsContainer {
	margin-top: 1em;
}
#requestAccessContainer textarea, #denyCommentContainer textarea {
	width: 32.5em;
}
.JoinUsMessage {
	background-color: #FFF0CC;
	border: solid 1px #FFC536;
	padding: .5em;
	margin-bottom: 1em;
}
.JoinUsMessage .SuccessMessage {
	background-color: #FFF0CC;
	color: Black;
}
#DownloadInfo {
	margin-top: 1em;
}
#DownloadInfo > div {
	padding-top: .25em;
}
#DownloadInfo .ActivityLabelCell {
	width: 5em;
	vertical-align: top;
	padding-bottom: .25em;
}
#DownloadInfo .ActivityLabel {
	color: white;
	font-weight: normal;
	text-transform: uppercase;
	background-color: #CCCCC2;
	padding: .1em .8em .1em .3em;
}
#DownloadInfo .ActivityData {
	vertical-align: top;
	padding-left: .25em;
}
#DownloadInfo .HelpBubble {
	vertical-align: bottom;
}
#CurrentReleaseInfo .DownloadButton {
	margin: 0 auto;
}
.RightSideBar .AlternateBackground {
	background-color: #E5E5E0;
}
.DownloadButton {
	width: 200px;
}
.SmallDownloadButton {
	width: 156px;
}
.DownloadButton a {
	display: block;
	height: 32px;
	border: none;
	color: #fff;
	font-size: 1.3em;
	font-weight: bold;
	letter-spacing: 0.5px;
	line-height: 28px;
	padding-left: 16px;
	text-align: center;
	background: url('http://i2.codeplex.com/Images/v17672/btn_download.png') 0px 0px no-repeat;
	text-decoration: none;
}
.SmallDownloadButton a {
	display: block;
	height: 25px;
	border: none;
	color: #fff;
	font-size: 1.1em;
	font-weight: bold;
	letter-spacing: 0.5px;
	line-height: 22px;
	padding-left: 12px;
	text-align: center;
	background: url('http://i2.codeplex.com/Images/v17672/btn_download_small.png') 0px 0px no-repeat;
	text-decoration: none;
}
.DownloadButton a:hover, .DownloadButton a:active {
	color: #fff;
	background: url('http://i2.codeplex.com/Images/v17672/btn_download.png') 0px -32px no-repeat;
	text-decoration: none;
}
.SmallDownloadButton a:hover, .SmallDownloadButton a:active {
	color: #fff;
	background: url('http://i2.codeplex.com/Images/v17672/btn_download_small.png') 0px -25px no-repeat;
	text-decoration: none;
}
#ProjectActivity td {
	padding: 0 .25em 0 .25em;
}
.UserAvatarContent {
	float: left;
}
.IE .UserAvatarContent {
	width: 95%;
}
.UserAvatar {
	width: 50px;
	height: 50px;
	float: left;
	margin-right: .5em;
}
.UserDetails {
	float: left;
	vertical-align: top;
	margin-top: .5em;
}
h2 {
	color: Black;
}
#WikiVersions td {
	padding: .25em 2em .25em 0;
}
#WikiVersions th {
	padding: 0 2em 0 0;
}
.CommentsHeader {
	background-color: #ECECEC;
	font-weight: bolder;
	padding: .2em;
}
.WikiMarkup {
	color: gray;
	font-family: Consolas,Courier New,Courier,monospace;
}
#WikiPageTitle {
	width: 500px;
}
.commentTextBox {
	color: #333;
	border: solid .1em #A5ACB2;
	width: 400px;
	height: 10em;
	vertical-align: bottom;
	padding: .13em;
}
.wikiTop td {
	background-color: #ECECEC;
	padding: 0.3em;
}
.wikiSource {
	padding-top: 1em;
	padding-bottom: 1em;
	width: 100%;
}
.WikiInfo {
	clear: both;
}
.breadcrumbs {
	font-size: .8em;
	padding-left: 0;
	padding-top: .13em;
	text-align: left;
	color: rgb(50%,50%,50%);
}
.breadcrumbs a {
	padding: 0;
}
table.StandardPaddingRight th {
	background-color: #E2F6CC;
	text-align: left;
}
.IE .WikiContent {
	padding-bottom: 1.5em;
}
.FF .WikiContent table {
	margin-left: 1px;
	margin-top: 1px;
}
div.QuickWikiGuide {
}
div.QuickWikiGuide .Link {
	font-size: .8em;
	margin-top: .31em;
}
div.QuickWikiGuide .Markup {
	color: #777;
	margin: .1em;
}
div.wikidoc h1, div.wikidoc h2, div.wikidoc h3, div.wikidoc h4, div.wikidoc h5, div.wikidoc h6 {
	margin-bottom: .13em;
}
div.wikidoc h1 {
	font-size: 2.0em;
}
div.wikidoc h2 {
	font-size: 1.6em;
}
div.wikidoc h3 {
	font-size: 1.4em;
}
div.wikidoc h4 {
	font-size: 1.2em;
}
html.FF div.wikidoc .externalLink, html.IE6 div.wikidoc .externalLink, html.Opera div.wikidoc .externalLink, html.Safari div.wikidoc .externalLink {
	background-image: url(http://i2.codeplex.com/Images/v17672/icons_weblink_sm.gif);
	background-repeat: no-repeat;
	background-position: right 0;
	padding-right: 1.5em;
	padding-bottom: .5em;
}
html.IE6 div.wikidoc .externalLink {
	vertical-align: bottom;
	display: inline-block;
}
html.IE7 div.wikidoc .externalLinkIcon {
	background-image: url(http://i2.codeplex.com/Images/v17672/icons_weblink_sm.gif);
	background-repeat: no-repeat;
	background-position: right 0;
	display: inline-block;
	height: 1em;
	margin-left: .13em;
	width: .81em;
	padding-top: .5em;
	padding-left: .5em;
}
div.wikidoc pre, div.wikidoc div.csharpcode {
	background-color: #ECECEC !important;
	border: dashed .1em #3E62A6 !important;
	font-family: Consolas,"+"\"Courier New\""+@",Courier,Monospace !important;
	font-size: 1em !important;
	margin-top: 0;
	padding: .5em;
	padding-bottom: 1.5em;
	height: auto;
	overflow: auto;
	width: 100% !important;
}
.IE6 .wikidoc pre, .IE6 .wikidoc div.csharpcode {
	width: 96% !important;
}
.Safari .wikidoc pre, .Safari div.wikidoc span.codeInline, .Safari .wikidoc div.csharpcode {
	font-size: 1.25em;
}
div.wikidoc .csharpcode .rem {
	color: Green;
}
div.wikidoc .csharpcode .kwrd {
	color: Blue;
}
div.wikidoc .csharpcode .str {
	color: #A31515;
}
div.wikidoc .csharpcode .op {
	color: #0000c0;
}
div.wikidoc .csharpcode .preproc {
	color: Blue;
}
div.wikidoc .csharpcode .asp {
	background-color: Yellow;
}
div.wikidoc .csharpcode .html {
	color: #A31515;
}
div.wikidoc .csharpcode .attr {
	color: Red;
}
div.wikidoc .csharpcode .lnum {
	color: #606060;
}
div.wikidoc .csharpcode pre, div.wikidoc pre pre {
	background-color: transparent !important;
	border: none !important;
	padding: 0;
	margin: 0;
	font-size: 1em;
}
.IE6 .wikidoc .csharpcode pre {
	width: 100% !important;
}
div.wikidoc span.codeInline {
	font-family: Consolas," + "\"Courier New\"" + @",Courier,Monospace;
}
div.wikidoc table {
	border: solid .1em #BBB;
	border-collapse: collapse;
}
div.wikidoc th {
	font-weight: bold;
}
div.wikidoc td, div.wikidoc th {
	padding: .25em;
}
div.wikidoc td, div.wikidoc th, div.wikidoc tr {
	border: solid .1em #BBB;
}
div.wikidoc span.unresolved, div.wikidoc div.error, div.wikidoc span.error {
	color: #F00;
	font-style: italic;
	font-size: .9em;
}
div.wikidoc div.error {
	white-space: pre;
}
div.wikidoc div.quote {
	border: dotted .1em #aaa;
	border-left: none;
	border-right: none;
	font-style: italic;
	margin: 1em 0em 2.5em 3em;
	padding: .2em;
}
div.wikidoc div.rss {
}
div.wikidoc div.rss .date {
	color: #777;
	font-size: .8em;
	margin: 0;
	padding: 0;
}
div.wikidoc div.rss .entry {
	margin: 1.25em .63em 1.25em .63em;
}
div.wikidoc div.rss .moreinfo {
	color: #777;
	font-size: .8em;
}
div.wikidoc div.rss .moreinfo a:active, div.wikidoc div.rss .moreinfo a:link, div.wikidoc div.rss .moreinfo a:visited {
	color: #999;
}
div.wikidoc div.rss .moreinfo a:hover {
	color: #CE8B10;
}
div.wikidoc div.rss p {
	margin: .38em 0 .13em 0;
}
div.wikidoc div.rss .title {
	font-size: 1.25em;
	font-weight: bold;
	margin-bottom: .1em;
}
div.wikidoc div.accentbar {
	color: #777;
	font-size: .75em;
	letter-spacing: .13em;
	text-align: center;
}
div.wikidoc div.accentbar .left {
	background-image: url(http://i2.codeplex.com/Images/v17672/wiki-accentbar-left.png);
	background-position: left;
	background-repeat: no-repeat;
	margin-right: -.25em;
	padding-left: 12.5em;
}
div.wikidoc div.accentbar .right {
	background-image: url(http://i2.codeplex.com/Images/v17672/wiki-accentbar-right.png);
	background-position: right;
	background-repeat: no-repeat;
	margin-left: -.5em;
	padding-right: 12.5em;
}
div.wikidoc div.video span.player {
	background-image: url(http://i2.codeplex.com/Images/v17672/video-error.png);
	background-position: center;
	background-repeat: no-repeat;
	text-align: center;
	vertical-align: middle;
}
div.wikidoc div.video .external {
	font-size: .8em;
}
div.wikidoc ul {
	list-style-image: url(http://i2.codeplex.com/Images/v17672/doublearrow.gif);
	list-style-type: none;
}
div.wikidoc ul ol {
	list-style-image: none;
}
div.wikidoc li {
	margin-bottom: .3em;
	margin-top: .3em;
	vertical-align: middle;
}
.NewCommentMarker {
	background-color: Yellow;
	padding: 0 0.5em 0 0.5em;
}
.EmptySearchResultsNotice {
	font-style: italic;
}
.TabStrip table td {
	white-space: nowrap;
	padding: .25em .5em .25em .5em;
	border: solid 1px #a0a0a0;
}
.TabStrip table td.ActiveTab {
	border-bottom: none;
}
.TabStrip table td.InactiveTab {
	background-image: url(http://i2.codeplex.com/Images/v17672/button_gradient.gif);
	background-position: left bottom;
	background-repeat: repeat-x;
}
.TabStrip table td.ComposeInactiveTab {
	border-right: none;
}
.TabStrip table td.PreviewInactiveTab {
	border-left: none;
}
.TabStrip table td.FullWidth {
	width: 100%;
	border: none;
	border-bottom: solid 1px #a0a0a0;
}
.TabStrip table td a.TabLink {
	color: #30332d;
	text-decoration: none;
	white-space: nowrap;
	display: block;
	cursor: pointer;
	height: 100%;
	padding-top: 1px;
	padding-bottom: 5px;
}
.TabContent {
	border: solid 1px #a0a0a0;
	border-top: none;
}
.StatsLogo {
	padding: .5em 0 .5em 0;
	margin-left: auto;
	margin-right: auto;
	display: block;
}
div.input {
	margin-top: 0.5em;
	margin-bottom: 0.5em;
}
.DocumentationMissingMessage {
	background-color: #FFF0CC;
	border: solid 1px #FFC536;
	padding: .5em;
}
.EditHeaderCell {
	font-weight: bold;
	width: 8em;
	text-align: right;
}
html, body {
	height: 100%;
}
body {
	margin: 0;
	padding: 0;
	width: 100%;
}
#CanvasTable {
	height: 100%;
	width: 100%;
}
#Canvas {
	position: relative;
	min-height: 100%;
	height: 100%;
	voice-family: "+"\\\"}\\\"" + @";
	voice-family: inherit;
	height: auto;
	width: 100%;
}
html > body #Canvas {
	height: auto;
}
#CanvasContent {
	padding: 0px;
	padding-bottom: 48px;
	display: block;
}
#CanvasFooter {
	width: 100%;
	padding: 0.5em 0 0.75em 0;
	text-align: center;
	border-top: solid 2px #529900;
	background-image: url('http://i2.codeplex.com/Images/v17672/subnav-gradient.png');
	background-repeat: repeat-x;
}
.OverflowHidden {
	overflow: hidden;
}
.PageIndexControl {
	display: -moz-inline-stack;
	display: inline;
}
.PageIndexControl span.Selected, .PageIndexControl span.Unselected {
	width: 1.6em;
	padding: .2em 0 .2em 0;
	display: inline-block;
	font-family: Segoe UI;
	font-weight: bold;
	font-size: smaller;
	color: #FFFFFF;
	text-align: center;
	text-decoration: none;
}
.PageIndexControl a.Link {
	text-decoration: none;
	color: #529900;
}
.PageIndexControl span.Unselected {
	background-color: #E5E5E0;
	cursor: pointer;
}
.PageIndexControl span.Selected {
	background-color: #529900;
}
.PageIndexControl span.PageActivity {
	margin-right: .25em;
}
</style>
";
                #endregion
                string header = "<html>\r\n<head>\r\n" + css + "\r\n</head>\r\n<body>\r\n<div class=\"wikidoc\">\r\n<div id=\"WikiContent\" class=\"WikiContent\">\r\n";
                string footer = "\r\n</div>\r\n</div>\r\n</body>\r\n</html>";
                hdoc = header + hdoc + footer;
                #endregion
            } else {
                #region === Change it from HTML code to Text code ===
                // line breaks
                hdoc = hdoc.Replace("<br>", "\r\n").Replace("<br/>", "\r\n").Replace("<br />", "\r\n");
                // paragraphs
                hdoc = hdoc.Replace("<p>", "").Replace("</p>", "\r\n\r\n");
                // headers
                hdoc = hdoc.Replace("<h1>", "\r\n").Replace("</h1>", "\r\n\r\n");
                hdoc = hdoc.Replace("<h2>", "\r\n").Replace("</h2>", "\r\n\r\n");
                hdoc = hdoc.Replace("<h3>", "\r\n").Replace("</h3>", "\r\n\r\n");
                hdoc = hdoc.Replace("<h4>", "\r\n").Replace("</h4>", "\r\n\r\n");
                hdoc = hdoc.Replace("<h5>", "\r\n").Replace("</h5>", "\r\n\r\n");
                hdoc = hdoc.Replace("<h6>", "\r\n").Replace("</h6>", "\r\n\r\n");
                // lists
                hdoc = hdoc.Replace("<ul>", "\r\n\r\n").Replace("</ul>", "\r\n\r\n");
                hdoc = hdoc.Replace("<ol>", "\r\n\r\n").Replace("</ol>", "\r\n\r\n");
                hdoc = hdoc.Replace("<li>", "    - ").Replace("</li>", "\r\n");
                // font changes
                hdoc = hdoc.Replace("<b>", "").Replace("</b>", "");
                hdoc = hdoc.Replace("<i>", "").Replace("</i>", "");
                hdoc = hdoc.Replace("<strong>", "").Replace("</strong>", "");
                // Special Characters
                hdoc = hdoc.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&nbsp;", " ");
                hdoc = hdoc.Replace("&quot;", "\"").Replace("&#39;", "\'");
                // Images
                {
                    while (hdoc.Contains("<img "))
                    {
                        string startText = "<img ";
                        string endText = "/>";
                        int startPoint = hdoc.IndexOf(startText);
                        int endPoint = hdoc.IndexOf(endText, startPoint + startText.Length);
                        string stringtotakeout = hdoc.Substring(startPoint, endPoint - startPoint + endText.Length);
                        hdoc = hdoc.Replace(stringtotakeout, " [Image cannot be displayed in Rawr, please see full page in Browser] ");
                    }
                }
                // Links
                {
                    while (hdoc.Contains("<a href="))
                    {
                        string startText = "<a href=";
                        string endText = ">";
                        int startPoint = hdoc.IndexOf(startText);
                        int endPoint = hdoc.IndexOf(endText, startPoint + startText.Length);
                        string stringtotakeout = hdoc.Substring(startPoint, endPoint - startPoint + endText.Length);
                        hdoc = hdoc.Replace(stringtotakeout, ""); //" [Link cannot be accessed in Rawr, please see full page in Browser] ");
                    }
                    hdoc = hdoc.Replace("</a>", "");
                }

                // Refix extraneous spacing
                hdoc = hdoc.Replace("\n", "\r\n");
                while (hdoc.Contains("\r\r\n")) { hdoc = hdoc.Replace("\r\r\n", "\r\n"); }
                while (hdoc.Contains("\r\n\r\n\r\n")) { hdoc = hdoc.Replace("\r\n\r\n\r\n", "\r\n\r\n"); }
                while (hdoc.Contains("\r\n\r\n    -")) { hdoc = hdoc.Replace("\r\n\r\n    -", "\r\n    -"); }

                hdoc = hdoc.Trim();
                #endregion
            }

            // Pass it on
            if (this.GetWebHelpCompleted != null)
                this.GetWebHelpCompleted(this, new EventArgs<String>(hdoc));
        }

        private void _webClient_DownloadStringCompleted_WebData(object sender, DownloadStringCompletedEventArgs e) { _webClient_DownloadStringCompleted_Child(sender, e, true); }
        private void _webClient_DownloadStringCompleted_PlainText(object sender, DownloadStringCompletedEventArgs e) { _webClient_DownloadStringCompleted_Child(sender, e, false); }

        public event EventHandler<EventArgs<String>> GetWebHelpCompleted;
        public void GetWebHelpAsync(string identifier, bool dowebdata)
        {
            _lastIdentifier = identifier;
            string url = string.Format(URL_WEBHELP, identifier);
            if (dowebdata) {
                _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted_WebData);
            } else {
                _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted_PlainText);
            }
            _webClient.DownloadStringAsync(new Uri(url));
        }
    }
}
