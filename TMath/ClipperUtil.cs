using Clipper2Lib;
using System;
using System.Collections.Generic;
using tmath.geometryutils;

namespace tmath
{
    public static class ClipperUtil
    {
        public static PathD TPointsToClipperPathD(TPoint2DCollection t_pnts, double scale = 1.0)
        {
            double[] p = new double[t_pnts.Count * 2];
            for (int i = 0; i < t_pnts.Count; i++)
            {
                p[i * 2] = t_pnts[i].X;
                p[i * 2 + 1] = t_pnts[i].Y;
            }

            return Clipper.ScalePath(Clipper.MakePath(p), scale);
        }

        public static PathD TPoint2DCollectionToClipperPathD(TPointCollection<TPoint2D, TVector2D> t_pnts, double scale = 1.0)
        {
            double[] p = new double[t_pnts.Count * 2];
            for (int i = 0; i < t_pnts.Count; i++)
            {
                p[i * 2] = t_pnts[i].X;
                p[i * 2 + 1] = t_pnts[i].Y;
            }
            return Clipper.ScalePath(Clipper.MakePath(p), scale);
        }

        public static Path64 TPointsToClipperPath64(TPoint2DCollection t_pnts, double scale = 1.0)
        {
            long[] p = new long[t_pnts.Count * 2];
            for (int i = 0; i < t_pnts.Count; i++)
            {
                p[i * 2] = (long)Math.Round(t_pnts[i].X * scale);
                p[i * 2 + 1] = (long)Math.Round(t_pnts[i].Y * scale);
            }

            return Clipper.MakePath(p);
        }

        public static PathsD TPointsToClipperPathsD(List<TPoint2DCollection> t_pnts, double scale = 1.0)
        {
            PathsD pointDs = new PathsD();
            foreach (var pnts in t_pnts)
                pointDs.Add(TPointsToClipperPathD(pnts, scale));
            return pointDs;
        }

        public static Paths64 TPointsToClipperPaths64(List<TPoint2DCollection> t_pnts, double scale = 1.0)
        {
            Paths64 pointDs = new Paths64();
            foreach (var pnts in t_pnts)
                pointDs.Add(TPointsToClipperPath64(pnts, scale));
            return pointDs;
        }


        public static Path64 NFPPointsToCLipperPath64(List<NFPPoint> nfp_pnts, double scale = 1.0)
        {
            long[] p = new long[nfp_pnts.Count * 2];
            for (int i = 0; i < nfp_pnts.Count; i++)
            {
                p[i * 2] = (long)Math.Round((nfp_pnts[i].X * scale));
                p[i * 2 + 1] = (long)Math.Round(nfp_pnts[i].Y * scale);
            }

            return Clipper.MakePath(p);
        }
        public static PathD NFPPointsToCLipperPathD(List<NFPPoint> nfp_pnts, double scale = 1.0)
        {
            double[] p = new double[nfp_pnts.Count * 2];
            for (int i = 0; i < nfp_pnts.Count; i++)
            {
                p[i * 2] = (double)Math.Round((nfp_pnts[i].X * scale));
                p[i * 2 + 1] = (double)Math.Round(nfp_pnts[i].Y * scale);
            }

            return Clipper.MakePath(p);
        }

        public static Paths64 NFPPointsToCLipperPaths64(List<TreeNode<NFPPoint>> nfp_pnts, double scale = 1.0)
        {
            Paths64 pointDs = new Paths64();
            foreach (var pnts in nfp_pnts)
                pointDs.Add(NFPPointsToCLipperPath64(pnts, scale));
            return pointDs;
        }

        public static PathsD NFPPointsToCLipperPathsD(List<TreeNode<NFPPoint>> nfp_pnts, double scale = 1.0)
        {
            PathsD pointDs = new PathsD();
            foreach (var pnts in nfp_pnts)
                pointDs.Add(NFPPointsToCLipperPathD(pnts, scale));
            return pointDs;
        }

        public static TreeNode<NFPPoint> ClipperPathDToNFPPoints(PathD c_path, double scale = 1.0)
        {
            TreeNode<NFPPoint> points = new TreeNode<NFPPoint>();
            PathD c_scaled_path = Clipper.ScalePath(c_path, scale);
            foreach (var c_pnt in c_scaled_path)
                points.Add(new NFPPoint(c_pnt.x, c_pnt.y));
            return points;
        }

        public static TPoint2DCollection ClipperPathDToTPoints(PathD c_path, double scale = 1.0)
        {
            TPoint2DCollection points = new TPoint2DCollection();
            PathD c_scaled_path = Clipper.ScalePath(c_path, scale);
            foreach (var c_pnt in c_scaled_path)
                points.Add(new TPoint2D(c_pnt.x, c_pnt.y));
            return points;
        }
        public static TPoint2DCollection ClipperPathDToTPoint2DCollection(PathD c_path, double scale = 1.0)
        {
            TPoint2DCollection points = new TPoint2DCollection();
            PathD c_scaled_path = Clipper.ScalePath(c_path, scale);
            foreach (var c_pnt in c_scaled_path)
                points.Add(new TPoint2D(c_pnt.x, c_pnt.y));
            return points;
        }

        public static List<TPoint2DCollection> ClipperPathsDToTPoints(PathsD c_paths, double scale = 1.0)
        {
            List<TPoint2DCollection> result = new List<TPoint2DCollection>();
            foreach (var c_path in c_paths)
                result.Add(ClipperPathDToTPoints(c_path, scale));
            return result;
        }

        public static TPoint2DCollection ClipperPath64ToTPoints(Path64 c_path, double scale = 1.0)
        {
            TPoint2DCollection points = new TPoint2DCollection();
            Path64 c_scaled_path = Clipper.ScalePath(c_path, scale);
            foreach (var c_pnt in c_scaled_path)
                points.Add(new TPoint2D((double)c_pnt.X, (double)c_pnt.Y));
            return points;
        }

        public static List<TPoint2DCollection> ClipperPaths64ToTPoints(Paths64 c_paths, double scale = 1.0)
        {
            List<TPoint2DCollection> result = new List<TPoint2DCollection>();
            foreach (var c_path in c_paths)
                result.Add(ClipperPath64ToTPoints(c_path, scale));
            return result;
        }

        public static List<TPoint2DCollection> ClipperUnionTPoints(List<TPoint2DCollection> t_pnts, FillRule fr = FillRule.NonZero)
        {
            Clipper64 clipper = new Clipper64();
            foreach (var pnts in t_pnts)
            {
                clipper.AddSubject(TPointsToClipperPath64(pnts));
            }
            Paths64 solution = new Paths64();
            clipper.Execute(ClipType.Union, fr, solution);
            return ClipperPaths64ToTPoints(solution);
        }

        public static List<TPoint2DCollection> ClipperDiffTPoints(List<TPoint2DCollection> subject, List<List<TPoint2DCollection>> clips, FillRule fr = FillRule.NonZero)
        {
            Clipper64 clipper = new Clipper64();
            clipper.AddSubject(TPointsToClipperPaths64(subject));
            foreach (var clip in clips)
                clipper.AddClip(TPointsToClipperPaths64(clip));
            Paths64 solution = new Paths64();
            clipper.Execute(ClipType.Difference, fr, solution);
            return ClipperPaths64ToTPoints(solution);
        }
    }
}
