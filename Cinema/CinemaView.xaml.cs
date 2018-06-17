using Cinema.Components;
using Cinema.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP3D;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Cinema
{
    public sealed partial class CinemaView : UserControl
    {
        public static DependencyProperty SeatsProperty { get; private set; }

        private Vector3 _seatSize = new Vector3(50, 100, 35);
        private float _freeSpaceForSeat = 15;
        private float _backFreeSpace = 250;
        private float _sceneLength = 400;
        private float _sideFreeSize = 50;
        private float _height = 500;

        private Scene _scene;

        public Seat[,] Seats
        {
            get => GetValue(SeatsProperty) as Seat[,];
            set => SetValue(SeatsProperty, value);
        }

        static CinemaView()
        {
            SeatsProperty = DependencyProperty.Register("Seats", typeof(Seat[,]), typeof(CinemaView),
                new PropertyMetadata(null, SeatsChanged));
        }

        public CinemaView()
        {
            this.InitializeComponent();

            this.Loaded += CinemaView_Loaded;
            this.SizeChanged += CinemaView_SizeChanged;

            Windows.UI.Xaml.Media.CompositionTarget.Rendering += CompositionTarget_Rendering;

            UpdateClip();
        }

        private void UpdateClip()
        {
            Container.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, Container.ActualWidth, Container.ActualHeight) };
        }

        #region EVENT HANDLERS
        private static void SeatsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CinemaView cv = d as CinemaView;

            if(cv._scene != null)
                cv.CreateRoom();

            cv.BookingPlatform.Seats = cv.Seats;
        }

        private void CinemaView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(_scene?.Camera != null)
                _scene.Camera.GetBehaviour<CameraBase>().UpdateView();

            UpdateClip();
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            _scene.Update();
        }
        #endregion

        private async void CinemaView_Loaded(object sender, RoutedEventArgs e)
        {
            _scene = new Scene(Target);

#if DEBUG
            SceneObject so = new SceneObject();
            so.IsActive = true;
            so.AddBehaviour(new StopwatchBehaviour());

            _scene.AddObject(so);
#endif

            _scene.Camera.Transform.Position = new System.Numerics.Vector3(0, 125, 0);
            _scene.Camera.AddBehaviour(new SmoothMovementBehaviour());
            _scene.Camera.AddBehaviour(new LookAtBehaviour());

            if (Seats != null)
                CreateRoom();
        }

        private void CreateRoom()
        {
            int rows = Seats.GetLength(0);
            int columns = Seats.GetLength(1);

            float length = _backFreeSpace + (rows * (_freeSpaceForSeat + _seatSize.Z)) + _sceneLength;
            float height = _height;
            float width = _sideFreeSize + (columns * (_freeSpaceForSeat + _seatSize.X));
            width *= 2;

            #region ROOM
            {
                //left
                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                sceneObject.Transform.Position = new System.Numerics.Vector3(-width / 2, height, 0 - _backFreeSpace);
                sceneObject.Transform.Rotation = new System.Numerics.Vector3(0, Extensions.EulerToRadian(90), 0);
                renderer.Size = new System.Numerics.Vector2(length, height);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Color.FromArgb(255, 180, 180, 180));

                _scene.AddObject(sceneObject);
            }
            {
                //right
                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                sceneObject.Transform.Position = new System.Numerics.Vector3(width / 2, height, 0 - _backFreeSpace);
                sceneObject.Transform.Rotation = new System.Numerics.Vector3(0, Extensions.EulerToRadian(90), 0);
                renderer.Size = new System.Numerics.Vector2(length, height);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Color.FromArgb(255, 180, 180, 180));

                _scene.AddObject(sceneObject);
            }
            {
                //front
                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                sceneObject.Transform.Position = new System.Numerics.Vector3(-width / 2, height, length - _backFreeSpace);
                sceneObject.Transform.Rotation = new System.Numerics.Vector3(0, 0, 0);
                renderer.Size = new System.Numerics.Vector2(width, height);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Color.FromArgb(255, 140, 140, 140));

                _scene.AddObject(sceneObject);
            }
            {
                //bottom
                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                sceneObject.Transform.Position = new System.Numerics.Vector3(-width / 2, 0, 0 - _backFreeSpace);
                sceneObject.Transform.Rotation = new System.Numerics.Vector3(Extensions.EulerToRadian(90), 0, 0);
                renderer.Size = new System.Numerics.Vector2(width, length);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Color.FromArgb(255, 160, 160, 160));

                _scene.AddObject(sceneObject);
            }
            {
                //top
                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                sceneObject.Transform.Position = new System.Numerics.Vector3(-width / 2, height, 0 - _backFreeSpace);
                sceneObject.Transform.Rotation = new System.Numerics.Vector3(Extensions.EulerToRadian(90), 0, 0);
                renderer.Size = new System.Numerics.Vector2(width, length);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Color.FromArgb(255, 160, 160, 160));

                _scene.AddObject(sceneObject);
            }
            #endregion

            {
                //media
                Vector3 position = new Vector3(-350, 350, rows * (_freeSpaceForSeat + _seatSize.Z) + _backFreeSpace + _sceneLength * 0.95f);

                var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);
                renderer.Size = new Vector2(700, 300);
                renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Colors.White);
                renderer.EnableShadow();
                sceneObject.Transform.Position = position;

                _scene.Camera.GetBehaviour<LookAtBehaviour>().Target = new Vector3(0, 150, position.Z);

                _scene.AddObject(sceneObject);
            }

            #region SEATS
            for (int r = rows - 1; r > 0; r--)
            {
                for(int c = 0; c < columns; c++)
                {
                    var seat = Seats[r, c];

                    Vector3 position = new Vector3((c - columns / 2) * (_freeSpaceForSeat + _seatSize.X), 
                        _seatSize.X * 0.75f, 
                        r * (_freeSpaceForSeat + _seatSize.Z) + _backFreeSpace);

                    var (sceneObject, renderer) = Extensions.CreateSceneObjectWithRenderer<SpriteRenderer>(_scene.CurrentCompositor);

                    sceneObject.AddBehaviour(new SmoothMovementBehaviour());
                    sceneObject.AddBehaviour(new SeatBehaviour());

                    sceneObject.Transform.Position = position;
                    renderer.Size = new Vector2(_seatSize.X, _seatSize.Y / 3);
                    renderer.Brush = _scene.CurrentCompositor.CreateColorBrush(Randomizer.RandomColor());

                    var entity = new SeatEntity(seat);
                    entity.SelectionChanged += async delegate
                    {
                        if(position.Z > _scene.Camera.Transform.Position.Z)
                            await Task.Delay(1000);

                        if(entity.Value.IsSelected)
                        {
                            _scene.Camera.GetBehaviour<SmoothMovementBehaviour>().MoveTo(new Vector3(position.X, 125, position.Z));
                        }
                    };

                    sceneObject.AddEntity(entity);

                    _scene.AddObject(sceneObject);
                }
            }
            #endregion
        }
    }
}
