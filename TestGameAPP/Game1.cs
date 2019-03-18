using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BCGSA;

namespace TestGameAPP
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        AccelerometerEntity accelerometerEntity = new AccelerometerEntity(default(System.Numerics.Vector3), default(System.Numerics.Vector3));
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;
        BasicEffect effect;
        ReceiverBluetoothService receiverBluetoothService = new ReceiverBluetoothService();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 6), Vector3.Zero, Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)Window.ClientBounds.Width /
                (float)Window.ClientBounds.Height,
                1, 100);

            worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), new Vector3(0, 0, -1), Vector3.Up);

            // создаем набор вершин
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue);

            // Создаем буфер вершин
            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor),
                triangleVertices.Length, BufferUsage.None);
            // Создаем BasicEffect
            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;
            // установка буфера вершин
            vertexBuffer.SetData(triangleVertices);
            receiverBluetoothService.Start((acc) => accelerometerEntity = acc);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            worldMatrix *= Matrix.CreateTranslation(accelerometerEntity.Accelerometer.X * delta, 0, 0);
            worldMatrix *= Matrix.CreateTranslation(0, accelerometerEntity.Accelerometer.Y * delta, 0);
            worldMatrix *= Matrix.CreateTranslation(0, 0, accelerometerEntity.Accelerometer.Z * delta);
            //worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));

            worldMatrix *= Matrix.CreateRotationX(accelerometerEntity.Gyroscope.X * delta);
            worldMatrix *= Matrix.CreateRotationY(accelerometerEntity.Gyroscope.Y * delta);
            worldMatrix *= Matrix.CreateRotationZ(accelerometerEntity.Gyroscope.Z * delta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //установка матриц эффекта
            effect.World = worldMatrix;
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            // установка буфера вершин
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>
                    (PrimitiveType.TriangleStrip, triangleVertices, 0, 1);
            }

            base.Draw(gameTime);
        }
    }
}