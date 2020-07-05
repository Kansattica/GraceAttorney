using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.DependencyInjection;
using FNA.GraceAttorney.Engines;
using FNA.GraceAttorney.Messages;
using FNA.GraceAttorney.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNA.GraceAttorney
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
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_aspectRatio = GraphicsDevice.Viewport.AspectRatio;
			_oldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

			var worldBuilder = new WorldBuilder();
			worldBuilder.AddOrderedRenderer(new SpriteRenderer(_spriteBatch, _scaleFactor, _viewport));
			worldBuilder.AddOrderedRenderer(new DialogueRenderer(_spriteBatch, _viewport, MakeColorTexture()));

			worldBuilder.AddEngine(new KeyboardEngine());
			worldBuilder.AddEngine(new FullScreenEngine(_graphics, _windowSize));
			worldBuilder.AddEngine(new UpdateBackgroundEngine(Content));
			worldBuilder.AddEngine(new UpdateCharacterEngine(Content));
			worldBuilder.AddEngine(new UpdateDialogueEngine());
			worldBuilder.AddEngine(new ClearBackgroundEngine());
			worldBuilder.AddEngine(new SpriteMotionEngine());
			worldBuilder.AddEngine(new StartMotionEngine());
			worldBuilder.AddEngine(new TextAnimationEngine());
			worldBuilder.AddEngine(new FadeEngine());

			var bg = worldBuilder.CreateEntity();
			worldBuilder.SetComponent(bg, new BackgroundComponent());
			worldBuilder.SetComponent(bg, new OpacityComponent());
			worldBuilder.SetComponent(bg, new SpriteComponent());

			worldBuilder.RegisterDrawLayer(1);
			var character = worldBuilder.CreateEntity();
			worldBuilder.SetComponent(character, new CharacterComponent());
			worldBuilder.SetComponent(character, new OpacityComponent());
			worldBuilder.SetComponent(character, new SpriteComponent());

			worldBuilder.SendMessage(new NewBackgroundMessage(assetName: Path.Combine("Case1", "background")));
			worldBuilder.SendMessage(new NewCharacterMessage(assetName: Path.Combine("Case1", "birdcall")));

			worldBuilder.RegisterDrawLayer(2);
			var dialogueBox = worldBuilder.CreateEntity();
			worldBuilder.SetComponent(dialogueBox, new DialogueComponent());
			worldBuilder.SetComponent(dialogueBox, new AnimatedTextComponent() { CharactersPerSecond = 30, CharactersVisible = 0 });

			worldBuilder.SendMessage(new NewDialogueMessage(new DialogueComponent {
				ShowBox = true,
				Dialogue = "Ah, Lostokyoangeles. The greatest city in the world for telecommunications-based bird crime.\nWhat are we doin' in court again, Di?\nIs it free gavel day already? I'm gonna take so many of those little hammery friends home, then wait for tiny wooden nail day.",
				Layer = 2,
				Speaker = "Bird Call"
			}));

			_world = worldBuilder.Build();
			base.Initialize();
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
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			// Run game logic in here. Do NOT render anything here!
			_world.Update(gameTime.ElapsedGameTime.TotalSeconds);
			base.Update(gameTime);
		}

		private const float BackgroundHeight = 1080f;
		protected override void Draw(GameTime gameTime)
		{
			// Render stuff in here. Do NOT run game logic in here!
			_scaleFactor.Factor = GraphicsDevice.Viewport.Height / BackgroundHeight;

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
		private Point _oldWindowSize;
		private float _aspectRatio;


		// from https://stackoverflow.com/questions/8396677/uniformly-resizing-a-window-in-xna
		private void WindowSizeChanged(object sender, EventArgs e)
		{
			// Remove this event handler, so we don't call it when we change the window size in here
			Window.ClientSizeChanged -= new EventHandler<EventArgs>(WindowSizeChanged);

			if (Window.ClientBounds.Width != _oldWindowSize.X)
			{ // We're changing the width
			  // Set the new backbuffer size
				_graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
				_graphics.PreferredBackBufferHeight = (int)(Window.ClientBounds.Width / _aspectRatio);
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
