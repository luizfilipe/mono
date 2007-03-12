// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Authors:
//	Peter Dennis Bartok	(pbartok@novell.com)
//
//

using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Web.UI.WebControls {

	// CAS
	[AspNetHostingPermissionAttribute (SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermissionAttribute (SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	// attributes
#if NET_2_0
// Not until we actually have StyleConverter
//	[TypeConverter(typeof(System.Web.UI.WebControls.StyleConverter))]
#else
	[TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
#endif
	[ToolboxItem("")]
	public class Style : System.ComponentModel.Component, System.Web.UI.IStateManager 
	{
		internal const string BitStateKey = "_!SB";

		[Flags]
		public enum Styles 
		{
			BackColor	= 0x00000008,
			BorderColor	= 0x00000010,
			BorderStyle	= 0x00000040,
			BorderWidth	= 0x00000020,
			CssClass	= 0x00000002,
			Font		= 0x00000001,
			ForeColor	= 0x00000004,
			Height		= 0x00000080,
			Width		= 0x00000100,

			FontAll = 0xFE00,
			FontBold = 0x800,
			FontItalic = 0x1000,
			FontNames = 0x200,
			FontOverline = 0x4000,
			FontSize = 0x400,
			FontStrikeout = 0x8000,
			FontUnderline = 0x2000
		}

		#region Fields
		private int styles;
		private int stylesTraked;
		internal StateBag	viewstate;
		private FontInfo	fontinfo;
		private bool		tracking;
		bool _isSharedViewState;
#if NET_2_0
		private string		registered_class;
#endif
		#endregion	// Fields

		#region Public Constructors
		public Style()
		{
			viewstate = new StateBag ();
		}

		public Style(System.Web.UI.StateBag bag) 
		{
			viewstate = bag;
			if (viewstate == null)
				viewstate = new StateBag ();
			_isSharedViewState = true;
		}
		#endregion	// Public Constructors

		#region Public Instance Properties
#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Color), "")]
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Color BackColor 
		{
			get 
			{
				if (!CheckBit ((int) Styles.BackColor)) 
				{
					return Color.Empty;
				}

				return (Color)viewstate["BackColor"];
			}

			set 
			{
				viewstate["BackColor"] = value;
				SetBit ((int) Styles.BackColor);
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Color), "")]
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Color BorderColor 
		{
			get 
			{
				if (!CheckBit ((int) Styles.BorderColor)) 
				{
					return Color.Empty;
				}

				return (Color)viewstate["BorderColor"];
			}

			set 
			{
				viewstate["BorderColor"] = value;
				SetBit ((int) Styles.BorderColor);
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(BorderStyle.NotSet)]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public BorderStyle BorderStyle 
		{
			get 
			{
				if (!CheckBit ((int) Styles.BorderStyle)) 
				{
					return BorderStyle.NotSet;
				}

				return (BorderStyle)viewstate["BorderStyle"];
			}

			set 
			{
				viewstate["BorderStyle"] = value;
				SetBit ((int) Styles.BorderStyle);
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Unit), "")]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Unit BorderWidth 
		{
			get 
			{
				if (!CheckBit ((int) Styles.BorderWidth)) 
				{
					return Unit.Empty;
				}

				return (Unit)viewstate["BorderWidth"];
			}

			set 
			{
				if (value.Value < 0) 
				{
					throw new ArgumentOutOfRangeException("Value", value.Value, "BorderWidth must not be negative");
				}

				viewstate["BorderWidth"] = value;
				SetBit ((int) Styles.BorderWidth);
			}
		}

		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public string CssClass 
		{
			get 
			{
				if (!CheckBit ((int) Styles.CssClass)) 
				{
					return string.Empty;
				}

				return (string)viewstate["CssClass"];
			}

			set 
			{
				viewstate["CssClass"] = value;
				SetBit ((int) Styles.CssClass);
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public FontInfo Font 
		{
			get 
			{
				if (fontinfo == null) 
				{
					fontinfo = new FontInfo(this);
				}
				return fontinfo;
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Color), "")]
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(System.Web.UI.WebControls.WebColorConverter))]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Color ForeColor 
		{
			get 
			{
				if (!CheckBit ((int) Styles.ForeColor)) 
				{
					return Color.Empty;
				}

				return (Color)viewstate["ForeColor"];
			}

			set 
			{
				viewstate["ForeColor"] = value;
				SetBit ((int) Styles.ForeColor);
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Unit), "")]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Unit Height 
		{
			get 
			{
				if (!CheckBit ((int) Styles.Height)) 
				{
					return Unit.Empty;
				}

				return (Unit)viewstate["Height"];
			}

			set 
			{
				if (value.Value < 0) 
				{
					throw new ArgumentOutOfRangeException("Value", value.Value, "Height must not be negative");
				}

				viewstate["Height"] = value;
				SetBit ((int) Styles.Height);
			}
		}

#if !NET_2_0
		[Bindable(true)]
#endif
		[DefaultValue(typeof (Unit), "")]
		[NotifyParentProperty(true)]
		[WebSysDescription ("")]
		[WebCategory ("Appearance")]
		public Unit Width 
		{
			get 
			{
				if (!CheckBit ((int) Styles.Width)) 
				{
					return Unit.Empty;
				}

				return (Unit)viewstate["Width"];
			}

			set 
			{
				if (value.Value < 0) 
				{
					throw new ArgumentOutOfRangeException("Value", value.Value, "Width must not be negative");
				}

				viewstate["Width"] = value;
				SetBit ((int) Styles.Width);
			}
		}
		#endregion	// Public Instance Properties

		#region Protected Instance Properties
#if NET_2_0
		[Browsable (false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool IsEmpty 
#else
		protected internal virtual bool IsEmpty 
#endif
		{
			get 
			{
#if NET_2_0
				return (styles == 0 && RegisteredCssClass.Length == 0);
#else
				return (styles == 0);
#endif
			}
		}

		protected bool IsTrackingViewState 
		{
			get 
			{
				return tracking;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal StateBag ViewState 
		{
			get 
			{
				return viewstate;
			}
		}
		#endregion	// Protected Instance Properties

		#region Internal Instance Properties
		internal bool AlwaysRenderTextDecoration
		{
			get
			{
				if (viewstate["AlwaysRenderTextDecoration"] == null)
					return false;
				return (bool)viewstate["AlwaysRenderTextDecoration"];
			}
		    
			set
			{
				viewstate["AlwaysRenderTextDecoration"] = value;
			}
		}
		#endregion	// Internal Instance Properties
		
		#region Public Instance Methods
		public void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) 
		{
			AddAttributesToRender(writer, null);
		}

		public virtual void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer, WebControl owner)
		{
#if NET_2_0
			if (RegisteredCssClass.Length > 0) {
				if (CssClass.Length > 0)
					writer.AddAttribute (HtmlTextWriterAttribute.Class, CssClass + " " + RegisteredCssClass);
				else
					writer.AddAttribute (HtmlTextWriterAttribute.Class, RegisteredCssClass);
			}
			else 
#endif
			{
				if (CssClass.Length > 0)
					writer.AddAttribute (HtmlTextWriterAttribute.Class, CssClass);
#if NET_2_0
				CssStyleCollection col = new CssStyleCollection ();
				FillStyleAttributes (col, owner);
				foreach (string key in col.Keys) {
					writer.AddStyleAttribute (key, col [key]);
				}
#else
				WriteStyleAttributes (writer);
#endif
			}
		}

#if ONLY_1_1
		void WriteStyleAttributes (HtmlTextWriter writer) 
		{
			string s;
			Color		color;
			BorderStyle	bs;
			Unit		u;

			if (CheckBit ((int) Styles.BackColor)) {
				color = (Color)viewstate["BackColor"];
				if (!color.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(color));
			}

			if (CheckBit ((int) Styles.BorderColor)) {
				color = (Color)viewstate["BorderColor"];
				if (!color.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(color));
			}

			bool have_width = false;
			if (CheckBit ((int) Styles.BorderWidth)) {
				u = (Unit)viewstate["BorderWidth"];
				if (!u.IsEmpty) {
					if (u.Value > 0)
						have_width = true;
					writer.AddStyleAttribute (HtmlTextWriterStyle.BorderWidth, u.ToString());
				}
			}

			if (CheckBit ((int) Styles.BorderStyle)) {
				bs = (BorderStyle)viewstate["BorderStyle"];
				if (bs != BorderStyle.NotSet) 
					writer.AddStyleAttribute (HtmlTextWriterStyle.BorderStyle, bs.ToString());
				else {
					if (CheckBit ((int) Styles.BorderWidth))
						writer.AddStyleAttribute (HtmlTextWriterStyle.BorderStyle, "solid");
				}
			} else if (have_width) {
				writer.AddStyleAttribute (HtmlTextWriterStyle.BorderStyle, "solid");
			}

			if (CheckBit ((int) Styles.ForeColor)) {
				color = (Color)viewstate["ForeColor"];
				if (!color.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(color));
			}

			if (CheckBit ((int) Styles.Height)) {
				u = (Unit)viewstate["Height"];
				if (!u.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.Height, u.ToString());
			}

			if (CheckBit ((int) Styles.Width)) {
				u = (Unit)viewstate["Width"];
				if (!u.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.Width, u.ToString());
			}

			if (CheckBit ((int) Style.Styles.FontAll)) {
				// Fonts are a bit weird
				if (fontinfo.Name != string.Empty) {
					s = fontinfo.Names[0];
					for (int i = 1; i < fontinfo.Names.Length; i++)
						s += "," + fontinfo.Names[i];
					writer.AddStyleAttribute (HtmlTextWriterStyle.FontFamily, s);
				}

				if (fontinfo.Bold)
					writer.AddStyleAttribute (HtmlTextWriterStyle.FontWeight, "bold");

				if (fontinfo.Italic)
					writer.AddStyleAttribute (HtmlTextWriterStyle.FontStyle, "italic");

				if (!fontinfo.Size.IsEmpty)
					writer.AddStyleAttribute (HtmlTextWriterStyle.FontSize, fontinfo.Size.ToString());

				// These styles are munged into a attribute decoration
				s = string.Empty;

				if (fontinfo.Overline)
					s += "overline ";

				if (fontinfo.Strikeout)
					s += "line-through ";

				if (fontinfo.Underline)
					s += "underline ";

				s = (s != "") ? s : AlwaysRenderTextDecoration ? "none" : "";
				if (s != "")
					writer.AddStyleAttribute (HtmlTextWriterStyle.TextDecoration, s);
			}
		}
#endif

#if NET_2_0
		protected virtual void FillStyleAttributes (CssStyleCollection attributes, IUrlResolutionService urlResolver)
		{
			Color		color;
			BorderStyle	bs;
			Unit		u;

			if (CheckBit ((int) Styles.BackColor))
			{
				color = (Color)viewstate["BackColor"];
				if (!color.IsEmpty)
					attributes.Add (HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(color));
			}

			if (CheckBit ((int) Styles.BorderColor)) 
			{
				color = (Color)viewstate["BorderColor"];
				if (!color.IsEmpty)
					attributes.Add (HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(color));
			}

			bool have_width = false;
			if (CheckBit ((int) Styles.BorderWidth)) {
				u = (Unit) viewstate ["BorderWidth"];
				if (!u.IsEmpty) {
					if (u.Value > 0)
						have_width = true;
					attributes.Add (HtmlTextWriterStyle.BorderWidth, u.ToString ());
				}
			}

			if (CheckBit ((int) Styles.BorderStyle)) {
				bs = (BorderStyle) viewstate ["BorderStyle"];
				if (bs != BorderStyle.NotSet)
					attributes.Add (HtmlTextWriterStyle.BorderStyle, bs.ToString ());
				else if (have_width)
						attributes.Add (HtmlTextWriterStyle.BorderStyle, "solid");
			}
			else if (have_width) {
				attributes.Add (HtmlTextWriterStyle.BorderStyle, "solid");
			}

			if (CheckBit ((int) Styles.ForeColor)) 
			{
				color = (Color)viewstate["ForeColor"];
				if (!color.IsEmpty)
					attributes.Add (HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(color));
			}

			if (CheckBit ((int) Styles.Height)) 
			{
				u = (Unit)viewstate["Height"];
				if (!u.IsEmpty)
					attributes.Add (HtmlTextWriterStyle.Height, u.ToString());
			}

			if (CheckBit ((int) Styles.Width)) 
			{
				u = (Unit)viewstate["Width"];
				if (!u.IsEmpty)
					attributes.Add (HtmlTextWriterStyle.Width, u.ToString());
			}

			Font.FillStyleAttributes (attributes, AlwaysRenderTextDecoration);
		}
#endif

		public virtual void CopyFrom(Style s) 
		{
			if ((s == null) || s.IsEmpty) 
			{
				return;
			}

			if (s.fontinfo != null) 
			{
				Font.CopyFrom(s.fontinfo);
			}

			if ((s.CheckBit ((int) Styles.BackColor)) && (s.BackColor != Color.Empty))
			{
				this.BackColor = s.BackColor;
			}
			if ((s.CheckBit ((int) Styles.BorderColor)) && (s.BorderColor != Color.Empty))
			{
				this.BorderColor = s.BorderColor;
			}
			if ((s.CheckBit ((int) Styles.BorderStyle)) && (s.BorderStyle != BorderStyle.NotSet))
			{
				this.BorderStyle = s.BorderStyle;
			}
			if ((s.CheckBit ((int) Styles.BorderWidth)) && (!s.BorderWidth.IsEmpty))
			{
				this.BorderWidth = s.BorderWidth;
			}
			if ((s.CheckBit ((int) Styles.CssClass)) && (s.CssClass != string.Empty))
			{
				this.CssClass = s.CssClass;
			}
			if ((s.CheckBit ((int) Styles.ForeColor)) && (s.ForeColor != Color.Empty))
			{
				this.ForeColor = s.ForeColor;
			}
			if ((s.CheckBit ((int) Styles.Height)) && (!s.Height.IsEmpty))
			{
				this.Height = s.Height;
			}
			if ((s.CheckBit ((int) Styles.Width)) && (!s.Width.IsEmpty))
			{
				this.Width = s.Width;
			}
		}

		public virtual void MergeWith(Style s) 
		{
			if ((s == null) || (s.IsEmpty))
			{
				return;
			}

			if (s.fontinfo != null) 
			{
				Font.MergeWith(s.fontinfo);
			}

			if ((!CheckBit ((int) Styles.BackColor)) && (s.CheckBit ((int) Styles.BackColor)) && (s.BackColor != Color.Empty))
			{
				this.BackColor = s.BackColor;
			}
			if ((!CheckBit ((int) Styles.BorderColor)) && (s.CheckBit ((int) Styles.BorderColor)) && (s.BorderColor != Color.Empty)) 
			{
				this.BorderColor = s.BorderColor;
			}
			if ((!CheckBit ((int) Styles.BorderStyle)) && (s.CheckBit ((int) Styles.BorderStyle)) && (s.BorderStyle != BorderStyle.NotSet))
			{
				this.BorderStyle = s.BorderStyle;
			}
			if ((!CheckBit ((int) Styles.BorderWidth)) && (s.CheckBit ((int) Styles.BorderWidth)) && (!s.BorderWidth.IsEmpty))
			{
				this.BorderWidth = s.BorderWidth;
			}
			if ((!CheckBit ((int) Styles.CssClass)) && (s.CheckBit ((int) Styles.CssClass)) && (s.CssClass != string.Empty))
			{
				this.CssClass = s.CssClass;
			}
			if ((!CheckBit ((int) Styles.ForeColor)) && (s.CheckBit ((int) Styles.ForeColor)) && (s.ForeColor != Color.Empty))
			{
				this.ForeColor = s.ForeColor;
			}
			if ((!CheckBit ((int) Styles.Height)) && (s.CheckBit ((int) Styles.Height)) && (!s.Height.IsEmpty))
			{
				this.Height = s.Height;
			}
			if ((!CheckBit ((int) Styles.Width)) && (s.CheckBit ((int) Styles.Width)) && (!s.Width.IsEmpty))
			{
				this.Width = s.Width;
			}
		}

		/*
		internal void Print ()
		{
			Console.WriteLine ("BackColor: {0}", BackColor);
			Console.WriteLine ("BorderColor: {0}", BorderColor);
			Console.WriteLine ("BorderStyle: {0}", BorderStyle);
			Console.WriteLine ("BorderWidth: {0}", BorderWidth);
			Console.WriteLine ("CssClass: {0}", CssClass);
			Console.WriteLine ("ForeColor: {0}", ForeColor);
			Console.WriteLine ("Height: {0}", Height);
			Console.WriteLine ("Width: {0}", Width);
		}
		*/

		public virtual void Reset() 
		{
			viewstate.Remove("BackColor");
			viewstate.Remove("BorderColor");
			viewstate.Remove("BorderStyle");
			viewstate.Remove("BorderWidth");
			viewstate.Remove("CssClass");
			viewstate.Remove("ForeColor");
			viewstate.Remove("Height");
			viewstate.Remove("Width");
			if (fontinfo != null) 
			{
				fontinfo.Reset();
			}
			styles = 0;
			viewstate.Remove (BitStateKey);
			stylesTraked = 0;
		}
#if ONLY_1_1
		public override string ToString() 
		{
			return string.Empty;
		}
#endif
		#endregion	// Public Instance Methods

		#region Protected Instance Methods
		protected internal void LoadViewState(object state) 
		{
			viewstate.LoadViewState(state);

			LoadBitState ();
		}

		protected internal virtual object SaveViewState () 
		{
			SaveBitState ();
			
			if (_isSharedViewState)
				return null;

			return viewstate.SaveViewState ();
			
		}

		internal void SaveBitState ()
		{
			if (stylesTraked != 0)
				viewstate [BitStateKey] = stylesTraked;
		}

		internal void LoadBitState () {
			if (viewstate [BitStateKey] == null)
				return;

			int bit = (int) viewstate [BitStateKey];
			styles |= bit;
			stylesTraked |= bit;
		}

		protected internal virtual void SetBit( int bit ) 
		{
			styles |= bit;
			if (tracking)
				stylesTraked |= bit;
		}

		internal void RemoveBit (int bit) {
			styles &= ~bit;
			if (tracking) {
				stylesTraked &= ~bit;
				if (stylesTraked == 0)
					viewstate.Remove (BitStateKey);
			}
		}

		internal bool CheckBit (int bit) {
			return (styles & bit) != 0;
		}

		protected internal virtual void TrackViewState () 
		{
			tracking = true;
			viewstate.TrackViewState();
		}
		#endregion	// Protected Instance Methods

		#region IStateManager Properties & Methods
		void IStateManager.LoadViewState(object state) 
		{
			LoadViewState(state);
		}

		object IStateManager.SaveViewState() 
		{
			return SaveViewState();
		}

		void IStateManager.TrackViewState() 
		{
			TrackViewState();
		}

		bool IStateManager.IsTrackingViewState 
		{
			get 
			{
				return this.IsTrackingViewState;
			}
		}
		#endregion	// IStateManager Properties & Methods

#if NET_2_0
		internal void SetRegisteredCssClass (string name)
		{
			registered_class = name;
		}

		public CssStyleCollection GetStyleAttributes (IUrlResolutionService resolver)
		{
			CssStyleCollection col = new CssStyleCollection ();
			FillStyleAttributes (col, resolver);
			return col;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string RegisteredCssClass {
			get {
				if (registered_class == null)
					registered_class = String.Empty;
				return registered_class;
			}
		}

		internal void CopyTextStylesFrom (Style source) {
			// Used primary for TreeView and Menu
			if (source.CheckBit ((int) Styles.ForeColor)) {
				ForeColor = source.ForeColor;
			}
			if (source.CheckBit((int) Styles.FontAll)) {
				Font.CopyFrom (source.Font);
			}
		}

		internal void RemoveTextStyles () {
			ForeColor = Color.Empty;
			fontinfo = null;
		}

		internal void AddCssClass (string cssClass) {
			if (String.IsNullOrEmpty (cssClass))
				return;

			if (CssClass.Length > 0)
				CssClass += " ";
			CssClass += cssClass;
		}

		public void SetDirty ()
		{
			if (viewstate != null)
				viewstate.SetDirty (true);
			stylesTraked = styles;
		}
#endif
	}
}
