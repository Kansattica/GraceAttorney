using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using Encompass;
using FNA.GraceAttorney.Components;
using FNA.GraceAttorney.Engines;
using FNA.GraceAttorney.Messages;
using FNA.GraceAttorney.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FNA.GraceAttorney
{
	class GraceAttorneyGame : Game
	{
		private World _world;
		public SpriteBatch SpriteBatch;
		public static GraceAttorneyGame Game;

		public GraceAttorneyGame()
		{
			// This gets assigned to something internally, don't worry...

			Graphics = new GraphicsDeviceManager(this)
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

			Game = this;
		}

		protected override void Initialize()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			_aspectRatio = GraphicsDevice.Viewport.AspectRatio;
			_oldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

			var worldBuilder = new WorldBuilder();
			worldBuilder.AddOrderedRenderer(new SpriteRenderer());
			worldBuilder.AddOrderedRenderer(new DialogueRenderer());

			worldBuilder.AddEngine(new KeyboardEngine());
			worldBuilder.AddEngine(new FullScreenEngine());
			worldBuilder.AddEngine(new UpdateBackgroundEngine());
			worldBuilder.AddEngine(new UpdateCharacterEngine());
			worldBuilder.AddEngine(new UpdateDialogueEngine());
			worldBuilder.AddEngine(new ClearBackgroundEngine());
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

			worldBuilder.SendMessage(new NewDialogueMessage(new DialogueComponent {
				ShowBox = true, Dialogue = "Ah, Lostokyoangeles. The greatest city in the world for telecommunications-based bird crime.\nWhat are we doin' in court again, Di?\nIs it free gavel day already? I'm gonna take so many of those little hammery friends home, then wait for tiny wooden nail day.", Layer = 2, Speaker = "Bird Call" }));

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
			SpriteBatch.Dispose();
			base.UnloadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			// Run game logic in here. Do NOT render anything here!
			ScaleFactor = GraphicsDevice.Viewport.Height / BackgroundHeight;
			_world.Update(gameTime.ElapsedGameTime.TotalSeconds);
			base.Update(gameTime);
		}

		private const float BackgroundHeight = 1080f;
		protected override void Draw(GameTime gameTime)
		{
			// Render stuff in here. Do NOT run game logic in here!

			GraphicsDevice.Clear(Color.Black);
			SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			_world.Draw();
			SpriteBatch.End();

			base.Draw(gameTime);
		}

		public float ScaleFactor { get; private set; }
		public GraphicsDeviceManager Graphics { get; private set; }
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
				Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
				Graphics.PreferredBackBufferHeight = (int)(Window.ClientBounds.Width / _aspectRatio);
			}
			else if (Window.ClientBounds.Height != _oldWindowSize.Y)
			{ // we're changing the height
			  // Set the new backbuffer size
				Graphics.PreferredBackBufferWidth = (int)(Window.ClientBounds.Height * _aspectRatio);
				Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
			}

			Graphics.ApplyChanges();

			// Update the old window size with what it is currently
			_oldWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

			// add this event handler back
			Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowSizeChanged);
		}
	}
}
