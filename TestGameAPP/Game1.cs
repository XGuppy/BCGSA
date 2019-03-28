using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCGSA;

namespace TestGameAPP
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        Matrix _projectionMatrix;
        Matrix _viewMatrix;
        Matrix _worldMatrix;
        AccelerometerEntity _accelerometerEntity = new AccelerometerEntity(default(System.Numerics.Vector3), default(System.Numerics.Vector3));
        VertexPositionColor[] _triangleVertices;
        VertexBuffer _vertexBuffer;
        BasicEffect _effect;
        readonly ReceiverBluetoothService _receiverBluetoothService = new ReceiverBluetoothService();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 6), Vector3.Zero, Vector3.Up);

            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)Window.ClientBounds.Width /
                (float)Window.ClientBounds.Height,
                1, 100);

            _worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), new Vector3(0, 0, -1), Vector3.Up);

            // создаем набор вершин
            _triangleVertices = new VertexPositionColor[3];
            _triangleVertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            _triangleVertices[1] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Green);
            _triangleVertices[2] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue);

            // Создаем буфер вершин
            _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
                _triangleVertices.Length, BufferUsage.None);
            // Создаем BasicEffect
            _effect = new BasicEffect(GraphicsDevice) {VertexColorEnabled = true};
            // установка буфера вершин
            _vertexBuffer.SetData(_triangleVertices);
            _receiverBluetoothService.Start((acc) => _accelerometerEntity = acc);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float delta = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            _worldMatrix *= Matrix.CreateTranslation((float)Math.Round(_accelerometerEntity.Accelerometer.X * delta, 1), 0, 0);
            _worldMatrix *= Matrix.CreateTranslation(0, (float)Math.Round(_accelerometerEntity.Accelerometer.Y * delta, 1), 0);
            _worldMatrix *= Matrix.CreateTranslation(0, 0, (float)Math.Round(_accelerometerEntity.Accelerometer.Z * delta, 1));
            //worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));

            _worldMatrix *= Matrix.CreateRotationX(_accelerometerEntity.Gyroscope.X * delta);
            _worldMatrix *= Matrix.CreateRotationY(_accelerometerEntity.Gyroscope.Y * delta);
            _worldMatrix *= Matrix.CreateRotationZ(_accelerometerEntity.Gyroscope.Z * delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //установка матриц эффекта
            _effect.World = _worldMatrix;
            _effect.View = _viewMatrix;
            _effect.Projection = _projectionMatrix;
            // установка буфера вершин
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);
            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives
                    (PrimitiveType.TriangleStrip, _triangleVertices, 0, 1);
            }

            base.Draw(gameTime);
        }
    }
}