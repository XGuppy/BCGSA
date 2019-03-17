using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;
        BasicEffect effect;

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

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                worldMatrix *= Matrix.CreateTranslation(-.01f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                worldMatrix *= Matrix.CreateTranslation(.01f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                worldMatrix *= Matrix.CreateTranslation(0, .01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                worldMatrix *= Matrix.CreateTranslation(0, -.01f, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                worldMatrix *= Matrix.CreateTranslation(0, 0, .01f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                worldMatrix *= Matrix.CreateTranslation(0, 0, -.01f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
            }

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