// BFPaperButton sample for Xamarin iOS
//
// Originally created by Bence Feher
// Ported to Xamarin iOS by Dominik Minta
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Bence Feher
// Copyright (c) 2016 Dominik Minta
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using BFPaperButtonXamarinIOS;
using UIKit;

namespace BFPaperButtonXamarinIOSExample
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.

			var bfFlatSmart = new BFPaperButton(new CoreGraphics.CGRect(20, 20, 280, 43), false);
			bfFlatSmart.SetTitle("BFPaperButton Flat: Smart Color", UIControlState.Normal);
			bfFlatSmart.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfFlatSmart.BackgroundColor = UIColor.FromRGBA(117f / 255f, 117f / 255f, 117f / 255f, 1f);
			bfFlatSmart.SetTitleColor(UIColor.White, UIControlState.Normal);
			bfFlatSmart.SetTitleColor(UIColor.White, UIControlState.Highlighted);
			bfFlatSmart.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfFlatSmart);

			var bfFlatDumb = new BFPaperButton(new CoreGraphics.CGRect(20, 71, 280, 43), false);
			bfFlatDumb.UsesSmartColor = false;
			bfFlatDumb.SetTitle("BFPaperButton Flat: !Smart Color", UIControlState.Normal);
			bfFlatDumb.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfFlatDumb.BackgroundColor = UIColor.FromRGBA(117f / 255f, 117f / 255f, 117f / 255f, 1f);
			bfFlatDumb.SetTitleColor(UIColor.White, UIControlState.Normal);
			bfFlatDumb.SetTitleColor(UIColor.White, UIControlState.Highlighted);
			bfFlatDumb.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfFlatDumb);

			var bfFlatClearSmart = new BFPaperButton(new CoreGraphics.CGRect(20, 122, 280, 43), false);
			bfFlatClearSmart.SetTitle("BFPaperButton Flat: Clear, Smart Color", UIControlState.Normal);
			bfFlatClearSmart.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfFlatClearSmart.SetTitleColor(UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f), UIControlState.Normal);
			bfFlatClearSmart.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfFlatClearSmart);

			var bfFlatClearDumb = new BFPaperButton(new CoreGraphics.CGRect(20, 173, 280, 43), false);
			bfFlatClearDumb.UsesSmartColor = false;
			bfFlatClearDumb.SetTitle("BFPaperButton Flat: Clear, !Smart Color", UIControlState.Normal);
			bfFlatClearDumb.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfFlatClearDumb.SetTitleColor(UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f), UIControlState.Normal);
			bfFlatClearDumb.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfFlatClearDumb);

			var bfRaisedSmart = new BFPaperButton(new CoreGraphics.CGRect(20, 239, 280, 43), true);
			bfRaisedSmart.BackgroundColor = UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f);
			bfRaisedSmart.SetTitle("BFPaperButton Raised: Smart Color", UIControlState.Normal);
			bfRaisedSmart.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfRaisedSmart.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfRaisedSmart);

			var bfRaisedDumb = new BFPaperButton(new CoreGraphics.CGRect(20, 307, 280, 43), true);
			bfRaisedDumb.UsesSmartColor = false;
			bfRaisedDumb.BackgroundColor = UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f);
			bfRaisedDumb.SetTitle("BFPaperButton Raised: !Smart Color", UIControlState.Normal);
			bfRaisedDumb.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfRaisedDumb.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfRaisedDumb);

			var bfRaisedSmartSmall = new BFPaperButton(new CoreGraphics.CGRect(20, 375, 135, 83), true);
			bfRaisedSmartSmall.BackgroundColor = UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f);
			bfRaisedSmartSmall.TitleLabel.Lines = 0;
			bfRaisedSmartSmall.Font = UIFont.SystemFontOfSize(10f);
			bfRaisedSmartSmall.SetTitle("BFPaperButton Raised: Smart Color", UIControlState.Normal);
			bfRaisedSmartSmall.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfRaisedSmartSmall.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfRaisedSmartSmall);

			var bfRaisedDumbSmall = new BFPaperButton(new CoreGraphics.CGRect(163, 375, 135, 83), true);
			bfRaisedDumbSmall.UsesSmartColor = false;
			bfRaisedDumbSmall.BackgroundColor = UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f);
			bfRaisedDumbSmall.TitleLabel.Lines = 0;
			bfRaisedDumbSmall.Font = UIFont.SystemFontOfSize(10f);
			bfRaisedDumbSmall.TapCircleDiameter = BFPaperButtonTapCircleDiameter.Full;
			bfRaisedDumbSmall.SetTitle("BFPaperButton Raised: !Smart Color, large circle", UIControlState.Normal);
			bfRaisedDumbSmall.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			bfRaisedDumbSmall.TouchUpInside += ButtonWasPressed;
			View.AddSubview(bfRaisedDumbSmall);

			var circle1 = new BFPaperButton(new CoreGraphics.CGRect(20, 468, 86, 86), true);
			circle1.BackgroundColor = UIColor.FromRGBA(33f / 255f, 150f / 255f, 243f / 255f, 1f);
			circle1.SetTitle("Center", UIControlState.Normal);
			circle1.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			circle1.SetTitleColor(UIColor.White, UIControlState.Normal);
			circle1.SetTitleColor(UIColor.White, UIControlState.Highlighted);
			circle1.TouchUpInside += ButtonWasPressed;
			circle1.CornerRadius = circle1.Frame.Size.Width / 2;
			circle1.RippleFromTapLocation = false;
			View.AddSubview(circle1);

			var circle2 = new BFPaperButton(new CoreGraphics.CGRect(116, 468, 86, 86), true);
			circle2.SetTitle("Center", UIControlState.Normal);
			circle2.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			circle2.SetTitleColor(UIColor.White, UIControlState.Normal);
			circle2.SetTitleColor(UIColor.White, UIControlState.Highlighted);
			circle2.TouchUpInside += ButtonWasPressed;
			circle2.BackgroundColor = UIColor.FromRGBA(0.3f, 0f, 1f, 1f);
			circle2.TapCircleColor = UIColor.FromRGBA(1f, 0f, 1f, 0.6f);
			circle2.CornerRadius = circle2.Frame.Size.Width / 2;
			circle2.RippleFromTapLocation = false;
			circle2.RippleBeyondBounds = true;
			circle2.TapCircleDiameter = (nfloat)Math.Max(circle2.Frame.Size.Width, circle2.Frame.Size.Height) * 1.3f;
			View.AddSubview(circle2);

			var circle3 = new BFPaperButton(new CoreGraphics.CGRect(212, 468, 86, 86), false);
			circle3.SetTitle("Center", UIControlState.Normal);
			circle3.SetTitleFont(UIFont.FromName("HelveticaNeue-Light", 15f));
			circle3.SetTitleColor(UIColor.FromRGBA(33f / 255f, 33f / 255f, 33f / 255f, 1f), UIControlState.Normal);
			circle3.SetTitleColor(UIColor.FromRGBA(33f / 255f, 33f / 255f, 33f / 255f, 1f), UIControlState.Highlighted);
			circle3.TouchUpInside += ButtonWasPressed;
			circle3.CornerRadius = circle3.Frame.Size.Width / 2;
			circle3.TapCircleDiameter = 53;
			circle3.TapCircleColor = UIColor.FromRGBA(0.3f, 0f, 1f, 0.6f);
			circle3.BackgroundFadeColor = UIColor.FromRGBA(0f, 0f, 1f, 0.3f);
			View.AddSubview(circle3);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		void ButtonWasPressed(object sender, EventArgs e)
		{
			Console.WriteLine("{0} was pressed!", ((UIButton)sender).TitleLabel.Text);
		}
	}
}

