using Clipper2Lib;
using System.Text.RegularExpressions;
using tmath;
using tmath.geo_math.curve;
using tmath.geometryutils;

namespace tmath_lab.algorithm_lab
{
    [TestClass]
    public class NFP_Lab
    {
        public void ReadText(out TPoint2DCollection Mbound, out TPoint2DCollection loudongpointsR)
        {
            TPoint2DCollection cur_col = null;
            string filename = "E:\\my-repo\\TMath\\tmath-lab\\algorithm_lab\\nfptest.txt";

            Mbound = new TPoint2DCollection();
            loudongpointsR = new TPoint2DCollection();

            string pattern = @"^\[.*\]"; ;
            string patternX = @"X\s*=\s*([-+]?[0-9]*\.?[0-9]+)";
            string patternY = @"Y\s*=\s*([-+]?[0-9]*\.?[0-9]+)";

            foreach (var line in File.ReadAllLines(filename))
            {
                if (line.Trim().Equals("Mbound"))
                {
                    cur_col = Mbound;
                    continue;
                }
                else if (line.Trim().Equals("loudongpointsR"))
                {
                    cur_col = loudongpointsR;
                    continue;
                }
                else if (line.Trim().Equals("MainNFP0"))
                {
                    break;
                }

                bool matches = Regex.IsMatch(line.Trim(), pattern);
                if (matches)
                {
                    var str_line = line.Trim();

                    // 提取 X 的值
                    Match matchX = Regex.Match(str_line, patternX);
                    // 提取 Y 的值
                    Match matchY = Regex.Match(str_line, patternY);
                    if (matchX.Success && matchY.Success)
                    {
                        double x = double.Parse(matchX.Groups[1].Value);
                        double y = double.Parse(matchY.Groups[1].Value);
                        cur_col?.Add(new TPoint2D(x, y));
                    }
                    Console.WriteLine(str_line);
                }

            }
            Console.Write("Success");
        }

        [TestMethod]
        public void Test_NoFitPolygon()
        {
            TPoint2DCollection Mbound, loudongpointsR;
            ReadText(out Mbound, out loudongpointsR);

            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(Mbound));
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(loudongpointsR));

            if (Mbound.IsClockwise()) Mbound.Reverse();
            if (loudongpointsR.IsClockwise()) loudongpointsR.Reverse();

            TreeNode<NFPPoint> A = new TreeNode<NFPPoint>();
            Mbound.ForEach(item => A.Add(new NFPPoint(item)));
            TreeNode<NFPPoint> B = new TreeNode<NFPPoint>();
            loudongpointsR.ForEach(item => B.Add(new NFPPoint(item)));


            var result = NFPUtil.NoFitPolygon(A, B, false, false, 1e-9);

            foreach (var nfp in result)
            {
                TPoint2DCollection solution = new TPoint2DCollection();
                nfp.ForEach(item => solution.Add(item.ToPoint2d()));
                SvgUtils.AddSolution(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(solution) }, false);
            }


            string filename = @"..\..\..\Test_NoFitPolygon.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }



        [TestMethod]
        public void Test_Circl_NoFitPolygon()
        {
            TPoint2DCollection Mbound, loudongpointsR;
            ReadText(out Mbound, out loudongpointsR);
            //Circle A
            //TCircle2D _A = new TCircle2D(new TPoint2D(0, 0), 10);
            //Circle B
            //TCircle2D _B = new TCircle2D(new TPoint2D(0, 0), 2);

            //Mbound = _A.Discretize(64)! as TPoint2DCollection;
            //loudongpointsR = _B.Discretize(64)! as TPoint2DCollection;



            SvgWriter svg = new SvgWriter();
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(Mbound));
            SvgUtils.AddSubject(svg, ClipperUtil.TPoint2DCollectionToClipperPathD(loudongpointsR));

            if (Mbound.IsClockwise()) Mbound.Reverse();
            if (loudongpointsR.IsClockwise()) loudongpointsR.Reverse();

            TreeNode<NFPPoint> A = new TreeNode<NFPPoint>();
            Mbound.ForEach(item => A.Add(new NFPPoint(item)));
            TreeNode<NFPPoint> B = new TreeNode<NFPPoint>();
            loudongpointsR.ForEach(item => B.Add(new NFPPoint(item)));


            var result = NFPUtil.NoFitPolygon(A, B, false, false, 1e-9);

            foreach (var nfp in result)
            {
                TPoint2DCollection solution = new TPoint2DCollection();
                nfp.ForEach(item => solution.Add(item.ToPoint2d()));
                SvgUtils.AddSolution(svg, new PathsD() { ClipperUtil.TPoint2DCollectionToClipperPathD(solution) }, false);
            }


            string filename = @"..\..\..\Test_Circl_NoFitPolygon.svg";
            SvgUtils.SaveToFile(svg, filename, FillRule.NonZero, 800, 600, 10);
            ClipperFileIO.OpenFileWithDefaultApp(filename);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_SegmentDistance()
        {

        }
    }
}
