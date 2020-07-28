using System;
using System.Collections.Generic;
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
		private readonly UpdatedSize _screenSize = new UpdatedSize();

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

			_spriteRenderTarget = new RenderTarget2D(GraphicsDevice, Common.Constants.BackgroundWidthInPixels, Common.Constants.BackgroundHeightInPixels);

			var worldBuilder = new WorldBuilder();

			for (int i = 0; i < (int)DrawLayers.VeryTop; i++)
				worldBuilder.RegisterDrawLayer(i);
			worldBuilder.AddGeneralRenderer(new SpriteTargetRenderer(GraphicsDevice, _spriteRenderTarget, _spriteBatch), (int)DrawLayers.StartSpriteDraw);
			worldBuilder.AddGeneralRenderer(new EndSpriteTargetRenderer(GraphicsDevice, _graphics, _spriteRenderTarget, _spriteBatch), (int)DrawLayers.EndSpriteDraw);
			worldBuilder.AddOrderedRenderer(new SpriteRenderer(_spriteBatch));
			worldBuilder.AddOrderedRenderer(new DialogueRenderer(_spriteBatch, MakeColorTexture(), _screenSize, _contentLoader));

			worldBuilder.AddEngine(new KeyboardEngine());
			worldBuilder.AddEngine(new FullScreenEngine(_graphics, _screenSize));
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
			worldBuilder.SendMessage(new CharacterEnterMessage(characterName: "Bird Call", pose: "standing", drawLocation: DrawLocation.Center));
			//worldBuilder.SendMessage(new CharacterEnterMessage(characterName: "Grace", pose: "plotting", drawLocation: DrawLocation.Center));

			var dialogueBox = worldBuilder.CreateEntity();
			worldBuilder.SetComponent(dialogueBox, new DialogueComponent());

			worldBuilder.SendMessage(new NewDialogueMessage(new DialogueComponent( 
				display: true,
				nameTagLocation: NameTagLocation.Left,
				dialogue: "What's up, gamers? It's me, a real live bird, coming at you live from Some Kinda Courtroom. I'm extremely gay and ready to fucking party. Anyways, first one to touch my butt gets three entire bird dollars.",
				justification: JustifyText.Left,
				textColor: Color.White,
				speaker: "Bird Call"
			), 30));

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

		protected override void Draw(GameTime gameTime)
		{
			// Render stuff in here. Do NOT run game logic in here!
			_screenSize.Width = _graphics.PreferredBackBufferWidth;
			_screenSize.Height = _graphics.PreferredBackBufferHeight;

			// the SpriteTargetRenderer and UITargetRenderers begin and end the sprite batch in their own way
			_world.Draw();
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private readonly GraphicsDeviceManager _graphics;
		private ContentLoader _contentLoader;
		private Point _oldWindowSize;
		private RenderTarget2D _spriteRenderTarget;
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

			_screenSize.Height = Window.ClientBounds.Height;
			_screenSize.Width = Window.ClientBounds.Width;

			// add this event handler back
			Window.ClientSizeChanged += new EventHandler<EventArgs>(WindowSizeChanged);
		}
	}
}
