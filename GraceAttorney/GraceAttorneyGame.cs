using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Encompass;
using GraceAttorney.Components;
using GraceAttorney.DependencyInjection;
using GraceAttorney.Engines;
using GraceAttorney.Messages;
using GraceAttorney.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraceAttorney
{
	class GraceAttorneyGame : Game
	{
		private World _world;
		private SpriteBatch _spriteBatch;
		private readonly UpdatedSize _viewport = new UpdatedSize();
		private readonly UpdatedSize _windowSize = new UpdatedSize();

		public GraceAttorneyGame()
		{
			// This gets assigned to something internally, don't worry...

			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 720,
				IsFullScreen = false,
				GraphicsProfile = GraphicsProfile.HiDef,
				PreferMultiSampling = true,
				SynchronizeWithVerticalRetrace = true
			};

			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowSizeChanged);
			Window.Title = "Grace Attorney";

			IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		private Texture2D MakeColorTexture()
		{
			var colorTexture = new Texture2D(GraphicsDevice, 1, 1);
			colorTexture.SetData(new[] { Color.White });
			return colorTexture;
		}

		protected override void Initialize()
		{
			_contentLoader = new ContentLoader(Content, "Case1");
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_aspectRatio = GraphicsDevice.Viewport.AspectRatio;
			_oldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

			var worldBuilder = new WorldBuilder();

			for (int i = 0; i <= (int)SpriteLayers.VeryTop; i++)
				worldBuilder.RegisterDrawLayer(i);
			worldBuilder.AddOrderedRenderer(new SpriteRenderer(_spriteBatch, _scaleFactor, _viewport));
			worldBuilder.AddOrderedRenderer(new DialogueRenderer(_spriteBatch, _viewport, MakeColorTexture()));

			worldBuilder.AddEngine(new KeyboardEngine());
			worldBuilder.AddEngine(new FullScreenEngine(_graphics, _windowSize));
			worldBuilder.AddEngine(new NewBackgroundEngine(_contentLoader));
			worldBuilder.AddEngine(new NewCharacterEngine(_contentLoader));
			worldBuilder.AddEngine(new SetUpSpriteEngine());
			worldBuilder.AddEngine(new CharacterExitEngine());
			worldBuilder.AddEngine(new SpriteAnimationEngine());
			worldBuilder.AddEngine(new CharacterExitByPositionEngine());
			worldBuilder.AddEngine(new CharacterExitByNameEngine());
			worldBuilder.AddEngine(new UpdateDialogueEngine());
			worldBuilder.AddEngine(new RemoveSpriteEngine());
			worldBuilder.AddEngine(new ClearBackgroundEngine());
			worldBuilder.AddEngine(new ClearAllCharactersEngine());
			worldBuilder.AddEngine(new SpriteMotionEngine());
			worldBuilder.AddEngine(new StartMotionEngine());
			worldBuilder.AddEngine(new TextAnimationEngine());
			worldBuilder.AddEngine(new FadeEngine());

			worldBuilder.SendMessage(new NewBackgroundMessage(assetName: "court"));
			//worldBuilder.SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", drawLocation: DrawLocation.Right));
			worldBuilder.SendMessage(new CharacterEnterMessage(characterName: "Grace", pose: "plotting", drawLocation: DrawLocation.Center));

			var dialogueBox = worldBuilder.CreateEntity();
			worldBuilder.SetComponent(dialogueBox, new DialogueComponent());
			worldBuilder.SetComponent(dialogueBox, new AnimatedTextComponent() { CharactersPerSecond = 30, CharactersVisible = 0 });

			worldBuilder.SendMessage(new NewDialogueMessage(new DialogueComponent {
				Display = true,
				NameTagLocation = NameTagLocation.Left,
				Dialogue = "Am I closer, dear, or just here to demonstrate Bigger Grace Theory? Hard to say.",
				Justification = JustifyText.Left,
				TextColor = Color.White,
				Speaker = "Grace"
			}));

			_world = worldBuilder.Build();
		}

		protected override void LoadContent()
		{
			// Load textures, sounds, and so on in here...
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			// Clean up after yourself!
			_spriteBatch.Dispose();
			_contentLoader.Dispose();
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			// Run game logic in here. Do NOT render anything here!
			_world.Update(gameTime.ElapsedGameTime.TotalSeconds);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Render stuff in here. Do NOT run game logic in here!
			_scaleFactor.Factor = (float)GraphicsDevice.Viewport.Height / Common.Constants.BackgroundHeightInPixels;

			_viewport.Height = GraphicsDevice.Viewport.Height;
			_viewport.Width = GraphicsDevice.Viewport.Width;

			_windowSize.Height = Window.ClientBounds.Height;
			_windowSize.Width = Window.ClientBounds.Width;

			GraphicsDevice.Clear(Color.Black);
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			_world.Draw();
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private readonly ScaleFactor _scaleFactor = new ScaleFactor();
		private readonly GraphicsDeviceManager _graphics;
		private ContentLoader _contentLoader;
		private Point _oldWindowSize;
		private float _aspectRatio;

		private const int MinimumWindowWidth = 1000;

		// from https://stackoverflow.com/questions/8396677/uniformly-resizing-a-window-in-xna
		private void WindowSizeChanged(object sender, EventArgs e)
		{
			// Remove this event handler, so we don't call it when we change the window size in here
			Window.ClientSizeChanged -= new EventHandler<EventArgs>(WindowSizeChanged);

			if (Window.ClientBounds.Width != _oldWindowSize.X)
			{ // We're changing the width
			  // Set the new backbuffer size
				int targetWidth = Math.Max(MinimumWindowWidth, Window.ClientBounds.Width);
				_graphics.PreferredBackBufferWidth = targetWidth;
				_graphics.PreferredBackBufferHeight = (int)(targetWidth / _aspectRatio);
			}
			else if (Window.ClientBounds.Height != _oldWindowSize.Y)
			{ // we're changing the height
			  // Set the new backbuffer size
				_graphics.PreferredBackBufferWidth = (int)(Window.ClientBounds.Height * _aspectRatio);
				_graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
			}

			_graphics.ApplyChanges();

			// Update the old window size with what it is currently
			_oldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

			// add this event handler back
			Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowSizeChanged);
		}
	}
}
