// BFPaperButton for Xamarin iOS
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
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace BFPaperButtonXamarinIOS
{
	public class BFPaperButton : UIButton
	{
		nfloat _cornerRadius;
		bool _isRaised;
		bool _rippleBeyondBounds;
		bool _rippleFromTapLocation;
		UIFont _titleFont;

		public UIColor ShadowColor { get; set; }
		public float LoweredShadowOpacity { get; set; }
		public nfloat LoweredShadowRadius { get; set; }
		public CGSize LoweredShadowOffset { get; set; }
		public float LiftedShadowOpacity { get; set; }
		public nfloat LiftedShadowRadius { get; set; }
		public CGSize LiftedShadowOffset { get; set; }
		public nfloat TouchDownAnimationDuration { get; set; }
		public nfloat TouchUpAnimationDuration { get; set; }
		public nfloat CornerRadius
		{
			get { return _cornerRadius; }
			set { SetCornerRadius(value); }
		}
		public nfloat TapCircleDiameterStartValue { get; set; }
		public nfloat TapCircleDiameter { get; set; }
		public nfloat TapCircleBurstAmount { get; set; }
		public UIColor TapCircleColor { get; set; }
		public UIColor BackgroundFadeColor { get; set; }
		public bool RippleFromTapLocation
		{
			get { return _rippleFromTapLocation; }
			set { SetRippleFromTapLocation(value); }
		}
		public bool RippleBeyondBounds
		{
			get { return _rippleBeyondBounds; }
			set { SetRippleBeyondBounds(value); }
		}
		public bool IsRaised
		{
			get { return _isRaised; }
			set { SetIsRaised(value); }
		}
		public bool UsesSmartColor { get; set; }
		public UIFont TitleFont
		{
			get { return _titleFont; }
			set { SetTitleFont(value); }
		}

		protected CGRect DownRect { get; set; }
		protected CGRect UpRect { get; set; }
		protected CGRect FadeAndClippingMaskRect { get; set; }
		protected CGPoint TapPoint { get; set; }
		protected bool LetGo { get; set; }
		protected CALayer BackgroundColorFadeLayer { get; set; }
		protected NSMutableArray RippleAnimationQueue { get; set; }
		protected NSMutableArray DeathRowForCircleLayers { get; set; }
		protected UIColor DumbTapCircleFillColor { get; set; }
		protected UIColor ClearBackgroundDumbTapCircleColor { get; set; }
		protected UIColor ClearBackgroundDumbFadeColor { get; set; }

		public override bool Enabled
		{
			get { return base.Enabled; }
			set { SetEnabled(value); }
		}

		public BFPaperButton() : base()
		{
			SetupRaised(true);
		}

		public BFPaperButton(CGRect frame) : base(frame)
		{
			SetupRaised(true);
		}

		public BFPaperButton(NSCoder decoder) : base(decoder)
		{
			SetupRaised(true);
		}

		public BFPaperButton(bool raised)
		{
			SetupRaised(raised);
		}

		public BFPaperButton(CGRect frame, bool raised) : base(frame)
		{
			SetupRaised(raised);
		}

		public override void SizeToFit()
		{
			base.SizeToFit();

			if (IsRaised)
			{
				DownRect = new CGRect(Bounds.X - LoweredShadowOffset.Width,
									  Bounds.Y + LoweredShadowOffset.Height,
									  Bounds.Size.Width + (2 * LoweredShadowOffset.Width),
									  Bounds.Size.Height + LoweredShadowOffset.Height);

				UpRect = new CGRect(Bounds.X - LiftedShadowOffset.Width,
									Bounds.Y + LiftedShadowOffset.Height,
									Bounds.Size.Width + (2 * LiftedShadowOffset.Width),
									Bounds.Size.Height + LiftedShadowOffset.Height);

				Layer.ShadowColor = ShadowColor.CGColor;
				Layer.ShadowOpacity = LetGo ? LoweredShadowOpacity : LiftedShadowOpacity;
				Layer.ShadowRadius = LetGo ? LoweredShadowRadius : LiftedShadowRadius;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(LetGo ? DownRect : UpRect, CornerRadius).CGPath;
				Layer.ShadowOffset = LoweredShadowOffset;
			}
			else
			{
				Layer.ShadowOpacity = 0f;
				BackgroundColorFadeLayer.Frame = new CGRect(Bounds.X, Bounds.Y, Bounds.Size.Width, Bounds.Size.Height);
				BackgroundColorFadeLayer.CornerRadius = CornerRadius;
			}

			FadeAndClippingMaskRect = new CGRect(Bounds.X, Bounds.Y, Bounds.Size.Width, Bounds.Size.Height);

			SetNeedsDisplay();
			Layer.SetNeedsDisplay();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (IsRaised)
			{
				DownRect = new CGRect(Bounds.X - LoweredShadowOffset.Width,
									  Bounds.Y + LoweredShadowOffset.Height,
									  Bounds.Size.Width + (2 * LoweredShadowOffset.Width),
									  Bounds.Size.Height + LoweredShadowOffset.Height);

				UpRect = new CGRect(Bounds.X - LiftedShadowOffset.Width,
									Bounds.Y + LiftedShadowOffset.Height,
									Bounds.Size.Width + (2 * LiftedShadowOffset.Width),
									Bounds.Size.Height + LiftedShadowOffset.Height);

				Layer.ShadowColor = ShadowColor.CGColor;
				Layer.ShadowOpacity = LetGo ? LoweredShadowOpacity : LiftedShadowOpacity;
				Layer.ShadowRadius = LetGo ? LoweredShadowRadius : LiftedShadowRadius;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(LetGo ? DownRect : UpRect, CornerRadius).CGPath;
				Layer.ShadowOffset = LoweredShadowOffset;
			}
			else
			{
				Layer.ShadowOpacity = 0f;
				BackgroundColorFadeLayer.Frame = new CGRect(Bounds.X, Bounds.Y, Bounds.Size.Width, Bounds.Size.Height);
				BackgroundColorFadeLayer.CornerRadius = CornerRadius;
			}

			FadeAndClippingMaskRect = new CGRect(Bounds.X, Bounds.Y, Bounds.Size.Width, Bounds.Size.Height);

			SetNeedsDisplay();
			Layer.SetNeedsDisplay();
		}

		public void SetEnabled(bool enabled)
		{
			base.Enabled = enabled;

			if (IsRaised)
			{
				if (!enabled)
				{
					Layer.ShadowOpacity = 0;
				}
				else
				{
					Layer.ShadowOpacity = LoweredShadowOpacity;
				}
			}

			SetNeedsDisplay();
		}

		public void SetIsRaised(bool isRaised)
		{
			_isRaised = isRaised;

			DownRect = new CGRect(Bounds.X - LoweredShadowOffset.Width,
								  Bounds.Y + LoweredShadowOffset.Height,
								  Bounds.Size.Width + (2 * LoweredShadowOffset.Width),
								  Bounds.Size.Height + LoweredShadowOffset.Height);

			UpRect = new CGRect(Bounds.X - LiftedShadowOffset.Width,
								Bounds.Y + LiftedShadowOffset.Height,
								Bounds.Size.Width + (2 * LiftedShadowOffset.Width),
								Bounds.Size.Height + LiftedShadowOffset.Height);

			if (isRaised)
			{
				Layer.ShadowColor = ShadowColor.CGColor;
				Layer.ShadowOpacity = LoweredShadowOpacity;
				Layer.ShadowRadius = LoweredShadowRadius;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(DownRect, CornerRadius).CGPath;
				Layer.ShadowOffset = LoweredShadowOffset;
			}
			else
			{
				Layer.ShadowOpacity = 0f;
			}
		}

		public void SetCornerRadius(nfloat cornerRadius)
		{
			_cornerRadius = cornerRadius;
			Layer.CornerRadius = cornerRadius;
			BackgroundColorFadeLayer.CornerRadius = cornerRadius;

			if (IsRaised)
			{
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(DownRect, CornerRadius).CGPath;
			}

			LayoutSubviews();
		}

		public void SetTitleFont(UIFont font)
		{
			if (_titleFont != null)
			{
				_titleFont = font;
				TitleLabel.Font = font;
			}
		}

		public void SetRippleFromTapLocation(bool rippleFromTapLocation)
		{
			if (_rippleFromTapLocation != rippleFromTapLocation)
			{
				_rippleFromTapLocation = rippleFromTapLocation;
			}
		}

		public void SetRippleBeyondBounds(bool rippleBeyondBounds)
		{
			if (_rippleBeyondBounds != rippleBeyondBounds)
			{
				_rippleBeyondBounds = rippleBeyondBounds;
			}
		}

		public void SetupRaised(bool isRaised)
		{
			LetGo = true;

			LoweredShadowOpacity = 0.5f;
			LoweredShadowRadius = 1.5f;
			LoweredShadowOffset = new CGSize(0, 1);

			LiftedShadowOpacity = 0.5f;
			LiftedShadowRadius = 4.5f;
			LiftedShadowOffset = new CGSize(2, 4);

			TouchDownAnimationDuration = 0.25f;
			TouchUpAnimationDuration = TouchDownAnimationDuration * 2.5f;

			UsesSmartColor = true;
			_isRaised = isRaised;
			_cornerRadius = 0;
			TapCircleColor = null;
			BackgroundFadeColor = null;
			ShadowColor = UIColor.FromWhiteAlpha(0.2f, 1.0f);
			RippleFromTapLocation = true;
			RippleBeyondBounds = false;
			TapCircleDiameterStartValue = 5f;
			TapCircleDiameter = BFPaperButtonTapCircleDiameter.Default;
			TapCircleBurstAmount = 100f;
			DumbTapCircleFillColor = UIColor.FromWhiteAlpha(0.1f, 0.16f);
			ClearBackgroundDumbTapCircleColor = UIColor.FromWhiteAlpha(0.3f, 0.12f);
			ClearBackgroundDumbFadeColor = UIColor.FromWhiteAlpha(0.3f, 0.12f);

			RippleAnimationQueue = new NSMutableArray();
			DeathRowForCircleLayers = new NSMutableArray();

			FadeAndClippingMaskRect = new CGRect(Bounds.X, Bounds.Y, Bounds.Size.Width, Bounds.Size.Height);
			BackgroundColorFadeLayer = new CALayer();
			BackgroundColorFadeLayer.Frame = FadeAndClippingMaskRect;
			BackgroundColorFadeLayer.CornerRadius = CornerRadius;
			BackgroundColorFadeLayer.BackgroundColor = UIColor.Clear.CGColor;
			BackgroundColorFadeLayer.Opacity = 0;
			Layer.InsertSublayer(BackgroundColorFadeLayer, 0);

			Layer.MasksToBounds = false;
			ClipsToBounds = false;

			Layer.NeedsDisplayOnBoundsChange = true;
			ContentMode = UIViewContentMode.Redraw;

			TapCircleColor = null;
			BackgroundFadeColor = null;

			if (isRaised)
			{
				DownRect = new CGRect(Bounds.X - LoweredShadowOffset.Width,
						  Bounds.Y + LoweredShadowOffset.Height,
						  Bounds.Size.Width + (2 * LoweredShadowOffset.Width),
						  Bounds.Size.Height + LoweredShadowOffset.Height);

				UpRect = new CGRect(Bounds.X - LiftedShadowOffset.Width,
									Bounds.Y + LiftedShadowOffset.Height,
									Bounds.Size.Width + (2 * LiftedShadowOffset.Width),
									Bounds.Size.Height + LiftedShadowOffset.Height);

				Layer.ShadowColor = ShadowColor.CGColor;
				Layer.ShadowOpacity = LoweredShadowOpacity;
				Layer.ShadowRadius = LoweredShadowRadius;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(LetGo ? DownRect : UpRect, CornerRadius).CGPath;
				Layer.ShadowOffset = LoweredShadowOffset;
			}
			else
			{
				Layer.ShadowOpacity = 0f;
			}

			TouchDown += PaperTouchDown;
			TouchUpInside += PaperTouchUp;
			TouchUpOutside += PaperTouchUp;
			TouchCancel += PaperTouchUp;

			var tapGestureRecognizer = new UITapGestureRecognizer();
			tapGestureRecognizer.ShouldReceiveTouch += ShouldReceiveTouch;
			AddGestureRecognizer(tapGestureRecognizer);

			SetAnimationDidStopSelector(new ObjCRuntime.Selector("animationDidStop:finished:"));
		}

		bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
		{
			var location = touch.LocationInView(this);
			TapPoint = location;
			return false;
		}

		void PaperTouchDown(object sender, EventArgs e)
		{
			LetGo = false;
			TouchDownAnimations();
		}

		void PaperTouchUp(object sender, EventArgs e)
		{
			LetGo = true;
			TouchUpAnimations();
		}

		[Export("animationDidStop:finished:")]
		void AnimationDidStop(CAAnimation theAnimation2, bool flag)
		{
			if ((theAnimation2.ValueForKey(new NSString("id")) as NSString).ToString() == "fadeCircleOut")
			{
				DeathRowForCircleLayers.GetItem<UIView>(0).RemoveFromSuperview();
				if (DeathRowForCircleLayers.Count > 0)
				{
					DeathRowForCircleLayers.RemoveObject(0);
				}
			}
		}

		void TouchDownAnimations()
		{
			LiftButton();
			FadeInBackgroundAndRippleTapCircle();
		}

		void TouchUpAnimations()
		{
			LowerButtonAndFadeOutBackground();
			BurstTapCircle();
		}

		void LiftButton()
		{
			if (IsRaised)
			{
				nfloat startRadius = LoweredShadowRadius;
				nfloat startOpacity = LoweredShadowOpacity;
				CGPath startPath = UIBezierPath.FromRoundedRect(DownRect, CornerRadius).CGPath;

				if (Layer.AnimationKeys != null && Layer.AnimationKeys.Length > 0)
				{
					startRadius = Layer.PresentationLayer.ShadowRadius;
					startOpacity = Layer.PresentationLayer.ShadowOpacity;
					startPath = Layer.PresentationLayer.ShadowPath;
				}

				CABasicAnimation increaseRadius = CABasicAnimation.FromKeyPath("shadowRadius");
				increaseRadius.From = NSNumber.FromNFloat(startRadius);
				increaseRadius.To = NSNumber.FromNFloat(LiftedShadowRadius);
				increaseRadius.Duration = TouchDownAnimationDuration;
				increaseRadius.FillMode = CAFillMode.Forwards;
				increaseRadius.RemovedOnCompletion = true;
				Layer.ShadowRadius = LiftedShadowRadius;

				CABasicAnimation shadowOpacityAnimation = CABasicAnimation.FromKeyPath("shadowOpacity");
				shadowOpacityAnimation.Duration = TouchDownAnimationDuration;
				shadowOpacityAnimation.From = NSNumber.FromNFloat(startOpacity);
				shadowOpacityAnimation.To = NSNumber.FromNFloat(LiftedShadowOpacity);
				shadowOpacityAnimation.FillMode = CAFillMode.Backwards;
				shadowOpacityAnimation.RemovedOnCompletion = true;
				Layer.ShadowOpacity = LiftedShadowOpacity;

				CABasicAnimation shadowPathAnimation = CABasicAnimation.FromKeyPath("shadowPath");
				shadowPathAnimation.Duration = TouchDownAnimationDuration;
				shadowPathAnimation.SetFrom(startPath);
				shadowPathAnimation.SetTo(UIBezierPath.FromRoundedRect(UpRect, CornerRadius).CGPath);
				shadowPathAnimation.FillMode = CAFillMode.Forwards;
				shadowPathAnimation.RemovedOnCompletion = true;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(UpRect, CornerRadius).CGPath;

				Layer.AddAnimation(increaseRadius, "shadowRadius");
				Layer.AddAnimation(shadowOpacityAnimation, "shadowOpacity");
				Layer.AddAnimation(shadowPathAnimation, "shadow");
			}
		}

		void FadeInBackgroundAndRippleTapCircle()
		{
			if (IsColorClear(BackgroundColor))
			{
				if (TapCircleColor == null)
				{
					TapCircleColor = UsesSmartColor ? TitleLabel.TextColor.ColorWithAlpha(ClearBackgroundDumbTapCircleColor.CGColor.Alpha) : ClearBackgroundDumbTapCircleColor;
				}

				if (BackgroundFadeColor == null)
				{
					BackgroundFadeColor = UsesSmartColor ? TitleLabel.TextColor.ColorWithAlpha(ClearBackgroundDumbFadeColor.CGColor.Alpha) : ClearBackgroundDumbFadeColor;
				}

				BackgroundColorFadeLayer.BackgroundColor = BackgroundFadeColor.CGColor;

				float startingOpacity = BackgroundColorFadeLayer.Opacity;

				if (BackgroundColorFadeLayer.AnimationKeys != null && BackgroundColorFadeLayer.AnimationKeys.Length > 0)
				{
					startingOpacity = BackgroundColorFadeLayer.PresentationLayer.Opacity;
				}

				CABasicAnimation fadeBackgroundDarker = CABasicAnimation.FromKeyPath("opacity");
				fadeBackgroundDarker.Duration = TouchDownAnimationDuration;
				fadeBackgroundDarker.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear);
				fadeBackgroundDarker.SetFrom(NSNumber.FromNFloat(startingOpacity));
				fadeBackgroundDarker.SetTo(NSNumber.FromNFloat(1));
				fadeBackgroundDarker.FillMode = CAFillMode.Forwards;
				fadeBackgroundDarker.RemovedOnCompletion = !false;
				BackgroundColorFadeLayer.Opacity = 1;

				BackgroundColorFadeLayer.AddAnimation(fadeBackgroundDarker, "animateOpacity");
			}
			else
			{
				if (TapCircleColor == null)
				{
					TapCircleColor = UsesSmartColor ? TitleLabel.TextColor.ColorWithAlpha(DumbTapCircleFillColor.CGColor.Alpha) : DumbTapCircleFillColor;
				}
			}

			nfloat tapCircleFinalDiameter = CalculateTapCircleFinalDiameter();

			var tapCircleLayerSizerView = new UIView(new CGRect(0, 0, tapCircleFinalDiameter, tapCircleFinalDiameter));
			tapCircleLayerSizerView.Center = RippleFromTapLocation ? TapPoint : new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());

			var startingRectSizerView = new UIView(new CGRect(0, 0, TapCircleDiameterStartValue, TapCircleDiameterStartValue));
			startingRectSizerView.Center = tapCircleLayerSizerView.Center;

			UIBezierPath startingCirclePath = UIBezierPath.FromRoundedRect(startingRectSizerView.Frame, TapCircleDiameterStartValue / 2f);

			var endingRectSizerView = new UIView(new CGRect(0, 0, tapCircleFinalDiameter, tapCircleFinalDiameter));
			endingRectSizerView.Center = tapCircleLayerSizerView.Center;

			UIBezierPath endingCirclePath = UIBezierPath.FromRoundedRect(endingRectSizerView.Frame, tapCircleFinalDiameter / 2f);

			CAShapeLayer tapCircle = new CAShapeLayer();
			tapCircle.FillColor = TapCircleColor.CGColor;
			tapCircle.StrokeColor = UIColor.Clear.CGColor;
			tapCircle.BorderColor = UIColor.Clear.CGColor;
			tapCircle.BorderWidth = 0;
			tapCircle.Path = startingCirclePath.CGPath;

			if (!RippleBeyondBounds)
			{
				CAShapeLayer mask = new CAShapeLayer();
				mask.Path = UIBezierPath.FromRoundedRect(FadeAndClippingMaskRect, CornerRadius).CGPath;
				mask.FillColor = UIColor.Black.CGColor;
				mask.StrokeColor = UIColor.Clear.CGColor;
				mask.BorderColor = UIColor.Clear.CGColor;
				mask.BorderWidth = 0;

				tapCircle.Mask = mask;
			}

			RippleAnimationQueue.AddObjects(new NSObject[] { tapCircle });
			Layer.InsertSublayerAbove(tapCircle, BackgroundColorFadeLayer);

			CABasicAnimation tapCircleGrowthAnimation = CABasicAnimation.FromKeyPath("path");
			tapCircleGrowthAnimation.Duration = TouchDownAnimationDuration;
			tapCircleGrowthAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
			tapCircleGrowthAnimation.SetFrom(startingCirclePath.CGPath);
			tapCircleGrowthAnimation.SetTo(endingCirclePath.CGPath);
			tapCircleGrowthAnimation.FillMode = CAFillMode.Forwards;
			tapCircleGrowthAnimation.RemovedOnCompletion = false;

			CABasicAnimation fadeIn = CABasicAnimation.FromKeyPath("opacity");
			fadeIn.Duration = TouchDownAnimationDuration;
			fadeIn.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear);
			fadeIn.SetFrom(NSNumber.FromNFloat(0));
			fadeIn.SetTo(NSNumber.FromNFloat(1));
			fadeIn.FillMode = CAFillMode.Forwards;
			fadeIn.RemovedOnCompletion = false;

			tapCircle.AddAnimation(tapCircleGrowthAnimation, "animatePath");
			tapCircle.AddAnimation(fadeIn, "opacityAnimation");
		}

		void LowerButtonAndFadeOutBackground()
		{
			if (IsRaised)
			{
				nfloat startRadius = LiftedShadowRadius;
				nfloat startOpacity = LiftedShadowOpacity;
				CGPath startPath = UIBezierPath.FromRoundedRect(UpRect, CornerRadius).CGPath;

				if (Layer.AnimationKeys != null && Layer.AnimationKeys.Length > 0)
				{
					startRadius = Layer.PresentationLayer.ShadowRadius;
					startOpacity = Layer.PresentationLayer.ShadowOpacity;
					startPath = Layer.PresentationLayer.ShadowPath;
				}

				CABasicAnimation decreaseRadius = CABasicAnimation.FromKeyPath("shadowRadius");
				decreaseRadius.SetFrom(NSNumber.FromNFloat(startRadius));
				decreaseRadius.SetTo(NSNumber.FromNFloat(LoweredShadowRadius));
				decreaseRadius.Duration = TouchUpAnimationDuration;
				decreaseRadius.FillMode = CAFillMode.Forwards;
				decreaseRadius.RemovedOnCompletion = true;
				Layer.ShadowRadius = LoweredShadowRadius;

				CABasicAnimation shadowOpacityAnimation = CABasicAnimation.FromKeyPath("shadowOpacity");
				shadowOpacityAnimation.Duration = TouchUpAnimationDuration;
				shadowOpacityAnimation.SetFrom(NSNumber.FromNFloat(startOpacity));
				shadowOpacityAnimation.SetTo(NSNumber.FromNFloat(LoweredShadowOpacity));
				shadowOpacityAnimation.FillMode = CAFillMode.Backwards;
				shadowOpacityAnimation.RemovedOnCompletion = true;
				Layer.ShadowOpacity = LoweredShadowOpacity;

				CABasicAnimation shadowAnimation = CABasicAnimation.FromKeyPath("shadowPath");
				shadowAnimation.Duration = TouchUpAnimationDuration;
				shadowAnimation.SetFrom(startPath);
				shadowAnimation.SetTo(UIBezierPath.FromRoundedRect(DownRect, CornerRadius).CGPath);
				shadowAnimation.FillMode = CAFillMode.Forwards;
				shadowAnimation.RemovedOnCompletion = true;
				Layer.ShadowPath = UIBezierPath.FromRoundedRect(DownRect, CornerRadius).CGPath;


				Layer.AddAnimation(shadowAnimation, "shadow");
				Layer.AddAnimation(decreaseRadius, "shadowRadius");
				Layer.AddAnimation(shadowOpacityAnimation, "shadowOpacity");
			}

			if (IsColorClear(BackgroundColor))
			{
				nfloat startingOpacity = BackgroundColorFadeLayer.Opacity;

				if (BackgroundColorFadeLayer.AnimationKeys != null && BackgroundColorFadeLayer.AnimationKeys.Length > 0)
				{
					startingOpacity = BackgroundColorFadeLayer.PresentationLayer.Opacity;
				}

				CABasicAnimation removeFadeBackgroundDarker = CABasicAnimation.FromKeyPath("opacity");
				removeFadeBackgroundDarker.Duration = TouchUpAnimationDuration;
				removeFadeBackgroundDarker.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.Linear);
				removeFadeBackgroundDarker.SetFrom(NSNumber.FromNFloat(startingOpacity));
				removeFadeBackgroundDarker.SetTo(NSNumber.FromNFloat(0));
				removeFadeBackgroundDarker.FillMode = CAFillMode.Forwards;
				removeFadeBackgroundDarker.RemovedOnCompletion = !false;
				BackgroundColorFadeLayer.Opacity = 0;

				BackgroundColorFadeLayer.AddAnimation(removeFadeBackgroundDarker, "animateOpacity");
			}
		}

		void BurstTapCircle()
		{
			nfloat tapCircleFinalDiameter = CalculateTapCircleFinalDiameter();
			tapCircleFinalDiameter += TapCircleBurstAmount;

			var endingRectSizerView = new UIView(new CGRect(0, 0, tapCircleFinalDiameter, tapCircleFinalDiameter));
			endingRectSizerView.Center = RippleFromTapLocation ? TapPoint : new CGPoint(Bounds.GetMidX(), Bounds.GetMidY());

			UIBezierPath endingCirclePath = UIBezierPath.FromRoundedRect(endingRectSizerView.Frame, tapCircleFinalDiameter / 2f);

			CAShapeLayer tapCircle = (RippleAnimationQueue.Count > 0) ? RippleAnimationQueue.GetItem<CAShapeLayer>(0) : null;
			if (tapCircle != null)
			{
				if (RippleAnimationQueue.Count > 0)
				{
					RippleAnimationQueue.RemoveObject(0);
				}
				DeathRowForCircleLayers.AddObjects(new NSObject[] { tapCircle });

				CGPath startingPath = tapCircle.Path;
				nfloat startingOpacity = tapCircle.Opacity;

				if (tapCircle.AnimationKeys != null && tapCircle.AnimationKeys.Length > 0)
				{
					startingPath = tapCircle.Path;
					startingOpacity = tapCircle.PresentationLayer.Opacity;
				}

				CABasicAnimation tapCircleGrowthAnimation = CABasicAnimation.FromKeyPath("path");
				tapCircleGrowthAnimation.Duration = TouchUpAnimationDuration;
				tapCircleGrowthAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
				tapCircleGrowthAnimation.SetFrom(startingPath);
				tapCircleGrowthAnimation.SetTo(endingCirclePath.CGPath);
				tapCircleGrowthAnimation.FillMode = CAFillMode.Forwards;
				tapCircleGrowthAnimation.RemovedOnCompletion = false;

				CABasicAnimation fadeOut = CABasicAnimation.FromKeyPath("opacity");
				fadeOut.SetValueForKey(new NSString("fadeCircleOut"), new NSString("id"));
				fadeOut.SetFrom(NSNumber.FromNFloat(startingOpacity));
				fadeOut.SetTo(NSNumber.FromNFloat(0f));
				fadeOut.Duration = TouchUpAnimationDuration;
				fadeOut.FillMode = CAFillMode.Forwards;
				fadeOut.RemovedOnCompletion = false;

				tapCircle.AddAnimation(tapCircleGrowthAnimation, "animatePath");
				tapCircle.AddAnimation(fadeOut, "opacityAnimation");
			}
		}

		nfloat CalculateTapCircleFinalDiameter()
		{
			nfloat finalDiameter = TapCircleDiameter;

			if (TapCircleDiameter == BFPaperButtonTapCircleDiameter.Full)
			{
				nfloat centerWidth = Frame.Size.Width;
				nfloat centerHeight = Frame.Size.Height;
				nfloat tapWidth = 2 * (nfloat)Math.Max(TapPoint.X, centerWidth - TapPoint.X);
				nfloat tapHeight = 2 * (nfloat)Math.Max(TapPoint.Y, centerHeight - TapPoint.Y);
				nfloat desiredWidth = RippleFromTapLocation ? tapWidth : centerWidth;
				nfloat desiredHeight = RippleFromTapLocation ? tapHeight : centerHeight;
				nfloat diameter = (nfloat)Math.Sqrt(Math.Pow(desiredWidth, 2) + Math.Pow(desiredHeight, 2));
				finalDiameter = diameter;
			}
			else if (TapCircleDiameter < BFPaperButtonTapCircleDiameter.Full)
			{
				finalDiameter = (nfloat)Math.Max(Frame.Size.Width, Frame.Size.Height);
			}

			return finalDiameter;
		}

		bool IsColorClear(UIColor color)
		{
			if (color == UIColor.Clear || color == null)
			{
				return true;
			}

			nint totalComponents = color.CGColor.NumberOfComponents;
			bool isGreyscale = (totalComponents == 2);
			nfloat[] components = color.CGColor.Components;
			if (components == null)
			{
				return true;
			}
			if (isGreyscale)
			{
				if (components[1] <= 0)
				{
					return true;
				}
			}
			else
			{
				if (components[3] <= 0)
				{
					return true;
				}
			}

			return false;
		}
	}
}
