using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System.Collections.Generic;
using System.IO;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3PenCS
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private bool flag;
        private float px, py;
        private float mySize;
        private List<float> vx = new List<float>();
        private List<float> vy = new List<float>();
        private List<Color> col = new List<Color>();
        private List<float> size = new List<float>();
        private bool IsChecked2;
        private Color myCol = Colors.Green;

        public MainWindow()
        {
            this.InitializeComponent();
            flag = false;
            px = 100;
            py = 100;
            mySize = 16;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            // New File 메뉴 아이템 클릭 처리
            vx.Clear();
            vy.Clear();
            size.Clear();
            col.Clear();
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Save 메뉴 아이템 클릭 처리
            using (StreamWriter sw = new StreamWriter("D:/2021/1.txt"))
            {
                for (int i = 0; i < vx.Count; i++)
                {
                    sw.WriteLine($"{vx[i]} {vy[i]} {col[i].R} {col[i].G} {col[i].B} {col[i].A} {size[i]}");
                }
            }
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            // Load 메뉴 아이템 클릭 처리
            vx.Clear(); vy.Clear(); size.Clear(); col.Clear();

            using (StreamReader sr = new StreamReader("D:/2021/1.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    vx.Add(float.Parse(parts[0]));
                    vy.Add(float.Parse(parts[1]));
                    col.Add(Color.FromArgb(byte.Parse(parts[5]), byte.Parse(parts[2]), byte.Parse(parts[3]), byte.Parse(parts[4])));
                    size.Add(float.Parse(parts[6]));
                }
            }
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            // Exit 메뉴 아이템 클릭 처리
            this.Close();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // 슬라이더 값 변경 처리
            mySize = (float)e.NewValue;
        }

        private void CanvasControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // 캔버스에 포인터 진입 처리
            ((CanvasControl)sender).Invalidate();
        }

        private void CanvasControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 캔버스에 포인터 누름 처리
            flag = true;
        }

        private void CanvasControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // 캔버스에서 포인터 뗌 처리
            flag = false;
            px = py = 0.0f;
            vx.Add(px);
            vy.Add(py);
            col.Add(myCol);
            size.Add(mySize);
        }

        private void CanvasControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // 캔버스에서 포인터 이동 처리
            var canvas = (CanvasControl)sender;
            px = (float)e.GetCurrentPoint(canvas).Position.X;
            py = (float)e.GetCurrentPoint(canvas).Position.Y;
            if (flag)
            {
                vx.Add(px);
                vy.Add(py);
                col.Add(myCol);
                size.Add(mySize);
                canvas.Invalidate();
            }
        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            // 캔버스 그리기 처리
            int n = vx.Count;
            if (n <= 2) return;

            for (int i = 1; i < n; i++)
            {
                if (vx[i] == 0.0 && vy[i] == 0.0)
                {
                    i++;
                    continue;
                }
                args.DrawingSession.DrawLine(vx[i - 1], vy[i - 1], vx[i], vy[i], col[i], size[i]);
                args.DrawingSession.FillCircle(vx[i - 1], vy[i - 1], size[i] / 2, col[i]);
                args.DrawingSession.FillCircle(vx[i], vy[i], size[i] / 2, col[i]);
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // 토글 버튼 클릭 처리
            if (IsChecked2)
            {
                IsChecked2 = false;
                ToggleButton.Label = "Disable";
                colorPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                IsChecked2 = true;
                ToggleButton.Label = "Enable";
                colorPanel.Visibility = Visibility.Visible;
            }
        }

        private void ColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            // 색상 선택기 색상 변경 처리
            myCol = args.NewColor;
        }
    }
}
