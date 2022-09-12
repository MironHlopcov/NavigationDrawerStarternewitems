using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MyFinToControl
{
    public class CustomPieSeries : PieSeries
    {
        private List<IList<ScreenPoint>> slicePoints = new List<IList<ScreenPoint>>();
        private double total;
        private IList<PieSlice> slices;
        public IList<PieSlice> CasSlices
        {
            get
            {
                return slices;
            }
            set
            {
                slices = value;
            }
        }

        public OxyColor UnVisebleFillColors { get; set; }

        public override void Render(IRenderContext rc)
        {
            slicePoints.Clear();
            if (Slices.Count == 0)
            {
                return;
            }

            CasSlices = Slices;

            total = slices.Sum((PieSlice slice) => slice.Value);
            if (Math.Abs(total) < double.Epsilon)
            {
                return;
            }

            double num = Math.Min(base.PlotModel.PlotArea.Width, base.PlotModel.PlotArea.Height) / 2.0;
            double num2 = num * (Diameter - ExplodedDistance);
            double num3 = num * InnerDiameter;
            double num4 = StartAngle;
           
            ScreenPoint screenPoint = new ScreenPoint((base.PlotModel.PlotArea.Left + base.PlotModel.PlotArea.Right) * 0.5, (base.PlotModel.PlotArea.Top + base.PlotModel.PlotArea.Bottom) * 0.5);
            foreach (PieSlice slice in slices)
            {
                List<ScreenPoint> list = new List<ScreenPoint>();
                List<ScreenPoint> list2 = new List<ScreenPoint>();
                double num5 = slice.Value / total * AngleSpan;
                double num6 = num4 + num5;
                double num7 = (slice.IsExploded ? (ExplodedDistance * num) : 0.0);
                //double num8 = num4 + num5 / 2.0;
                double num8 = StartAngle;
                double num9 = num8 * Math.PI / 180.0;

                //var sdf = Math.Sin(StartAngle - 90) * TickRadialLength;
                //TickHorizontalLength = num * (1 - Diameter) + Math.Abs(sdf);

                ScreenPoint item = new ScreenPoint(screenPoint.X + num7 * Math.Cos(num9), screenPoint.Y + num7 * Math.Sin(num9));
                while (true)
                {
                    bool flag = false;
                    if (num4 >= num6)
                    {
                        num4 = num6;
                        flag = true;
                    }

                    double num10 = num4 * Math.PI / 180.0;
                    ScreenPoint item2 = new ScreenPoint(item.X + num2 * Math.Cos(num10), item.Y + num2 * Math.Sin(num10));
                    list.Add(item2);
                    ScreenPoint item3 = new ScreenPoint(item.X + num3 * Math.Cos(num10), item.Y + num3 * Math.Sin(num10));
                    if (num3 + num7 > 0.0)
                    {
                        list2.Add(item3);
                    }

                    if (flag)
                    {
                        break;
                    }

                    num4 += AngleIncrement;
                }

                list2.Reverse();
                if (list2.Count == 0)
                {
                    list2.Add(item);
                }

                list2.Add(list[0]);
                List<ScreenPoint> list3 = list;
                list3.AddRange(list2);
                rc.DrawPolygon(list3, slice.ActualFillColor, Stroke, StrokeThickness, null, LineJoin.Bevel);
                slicePoints.Add(list3);
                if (OutsideLabelFormat != null)
                {
                    string text = string.Format(OutsideLabelFormat, new object[3]
                    {
                        slice.Value,
                        slice.Label,
                        slice.Value / total * 100.0
                    });
                    //int num11 = Math.Sign(Math.Cos(num9));
                    int num11 = 1;



                    //ScreenPoint screenPoint2 = new ScreenPoint(item.X + (num2 + TickDistance) * Math.Cos(num9), item.Y + (num2 + TickDistance) * Math.Sin(num9));
                    //ScreenPoint screenPoint3 = new ScreenPoint(screenPoint2.X + TickRadialLength * Math.Cos(num9), screenPoint2.Y + TickRadialLength * Math.Sin(num9));
                    //ScreenPoint screenPoint4 = new ScreenPoint(screenPoint3.X + TickHorizontalLength * (double)num11, screenPoint3.Y);

                    ScreenPoint screenPoint2 = new ScreenPoint(item.X + (num2 + TickDistance) * Math.Cos(num9), item.Y + (num2 + TickDistance) * Math.Sin(num9));
                    ScreenPoint screenPoint3 = new ScreenPoint(screenPoint2.X + TickRadialLength, screenPoint2.Y + TickRadialLength * Math.Sin(num9));
                    ScreenPoint screenPoint4 = new ScreenPoint(screenPoint3.X + TickHorizontalLength * (double)num11, screenPoint3.Y);

                    if (slice.Fill == UnVisebleFillColors)
                    {
                        text = "";
                       

                        screenPoint2 = new ScreenPoint(item.X + (num2 + TickDistance) * Math.Cos(num9), item.Y + (num2 + TickDistance) * Math.Sin(num9));
                        screenPoint3 = new ScreenPoint(screenPoint2.X + TickRadialLength, screenPoint2.Y + TickRadialLength * Math.Sin(num9));
                        screenPoint4 = new ScreenPoint(screenPoint3.X + TickHorizontalLength * (double)num11, screenPoint3.Y);

                        //screenPoint2 = new ScreenPoint(item.X + (num2 + 0) * Math.Cos(num9), item.Y + (num2 + 0) * Math.Sin(num9));
                        //screenPoint3 = new ScreenPoint(screenPoint2.X + 0 * Math.Cos(num9), screenPoint2.Y + 0 * Math.Sin(num9));
                        //screenPoint4 = new ScreenPoint(screenPoint3.X + 0 * (double)num11, screenPoint3.Y);

                    }
                   
                    rc.DrawLine(new ScreenPoint[3] { screenPoint2, screenPoint3, screenPoint4 }, base.ActualTextColor, 1.0, null, LineJoin.Bevel);
                    ScreenPoint p = new ScreenPoint(screenPoint4.X + TickLabelDistance * (double)num11, screenPoint4.Y);

                    ///Castom Label for default Fill
                    
                    rc.DrawText(p, text, base.ActualTextColor, base.ActualFont, base.ActualFontSize, base.ActualFontWeight, 0.0, (num11 <= 0) ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Middle);
                }

                if (InsideLabelFormat == null || InsideLabelColor.IsUndefined())
                {
                    continue;
                }

                string text2 = string.Format(InsideLabelFormat, new object[3]
                {
                    slice.Value,
                    slice.Label,
                    slice.Value / total * 100.0
                });
                double num12 = num3 * (1.0 - InsideLabelPosition) + num2 * InsideLabelPosition;
                ScreenPoint p2 = new ScreenPoint(item.X + num12 * Math.Cos(num9), item.Y + num12 * Math.Sin(num9));
                double num13 = 0.0;
                if (AreInsideLabelsAngled)
                {
                    num13 = num8;
                    if (Math.Cos(num9) < 0.0)
                    {
                        num13 += 180.0;
                    }
                }

                OxyColor fill = (InsideLabelColor.IsAutomatic() ? base.ActualTextColor : InsideLabelColor);
                rc.DrawText(p2, text2, fill, base.ActualFont, base.ActualFontSize, base.ActualFontWeight, num13, HorizontalAlignment.Center, VerticalAlignment.Middle);
            }


        }
    }
}