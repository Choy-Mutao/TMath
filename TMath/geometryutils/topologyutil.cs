using tmath.GeometryUtil;
using System;
using System.Collections.Generic;

namespace tmath.ComputeGeometry.GeometryUtils
{
    // 通用拓扑工具
    public static class TopologyUtil
    {
        private class VNode
        {
            public TPoint2D pnt { get; set; }
            //public ENode first_edge { get; SetByRowElements; }
            public List<VEdge> edgeOut { get; set; }
            //public int edge_num { get; SetByRowElements; }
            public VNode(TPoint2D p)
            {
                pnt = p;
                edgeOut = new List<VEdge>();
            }
            public double distTo(VNode other)
            {
                return GeometryUtil.CommonUtil.distance(pnt, other.pnt);
            }
            public bool equalTo(VNode other)
            {
                return NumberUtil.CompValue(distTo(other), 0) == 0;
            }
            public void addEdge(VEdge edge)
            {
                int i = 0;
                while (i < edgeOut.Count && edge.getTheta() > edgeOut[i].getTheta())
                    i += 1;
                edgeOut.Insert(i, edge);
            }
        };
        private class VEdge
        {
            public VEdge() { }
            public VEdge(VNode s, VNode e)
            {
                sv = s;
                ev = e;
                isVisited = false;
            }
            public VNode sv { get; set; }
            public VNode ev { get; set; }
            public bool isVisited { get; set; }
            public double getLength()
            {
                return sv.distTo(ev);
            }
            public VEdge getInverse()
            {
                return new VEdge(ev, sv);
            }
            public double getTheta()
            {
                double theta = 0;
                double cosX = (ev.pnt.X - sv.pnt.X) / getLength();
                double cosY = (ev.pnt.Y - sv.pnt.Y) / getLength();
                if (cosY > 0)
                    theta = Math.Acos(cosX);
                else
                    theta = 2 * Math.PI - Math.Acos(cosX);
                //if(GeometryUtil.Common.compareValue(theta, 2*Math.PI) == 0) theta = 0;
                return theta;
            }
        }
        private class TreeNode
        {
            public List<TreeNode> childs { get; set; }
            public List<int> cycle { get; set; }
            public TreeNode p_node { get; set; }
            public TreeNode()
            {
                childs = new List<TreeNode>();
                cycle = new List<int>();
                p_node = null;
            }
            public TreeNode(List<int> data, List<TreeNode> c, TreeNode p = null)
            {
                childs = c;
                cycle = data;
                p_node = p;
            }
        }

        /// <summary>
        /// 获得点集中所有的闭合关系
        /// </summary>
        /// <param name="points_set"></param>
        /// <returns></returns>
        public static List<List<KeyValuePair<TPoint2D, TPoint2D>>> GetAllLoops(List<KeyValuePair<TPoint2D, TPoint2D>> point_pairs)
        {
            List<VNode> vertices = new List<VNode>();
            List<bool> visited = new List<bool>();
            List<List<KeyValuePair<TPoint2D, TPoint2D>>> result = new List<List<KeyValuePair<TPoint2D, TPoint2D>>>();

            #region 构建节点和边数据
            vertices = new List<VNode>();
            int pnt_pair_size = point_pairs.Count;
            List<KeyValuePair<TPoint2D, TPoint2D>> copy_pnts = new List<KeyValuePair<TPoint2D, TPoint2D>>();
            for (int i = 0; i < pnt_pair_size; i++)
            {
                if (CommonUtil.IsEqualPoint(point_pairs[i].Key, point_pairs[i].Value)) continue; // 去重
                TPoint2D pnt1 = new TPoint2D(point_pairs[i].Key.X, point_pairs[i].Key.Y);
                TPoint2D pnt2 = new TPoint2D(point_pairs[i].Value.X, point_pairs[i].Value.Y);
                copy_pnts.Add(new KeyValuePair<TPoint2D, TPoint2D>(pnt1, pnt2));
            }
                
            for (int i = 0; i < copy_pnts.Count; i++)
            {
                KeyValuePair<TPoint2D, TPoint2D> pair = copy_pnts[i];

                double length = pair.Key.DistanceTo(pair.Value);
                if (NumberUtil.CompValue(length, 0, 0.1) == 0) continue;

                VNode s = new VNode(pair.Key);
                VNode e = new VNode(pair.Value);
                bool eIsNew = true, sIsNew = true;
                foreach (VNode v in vertices)
                {
                    if (v.equalTo(s))
                    {
                        s = v;
                        sIsNew = false;
                    }
                    if (v.equalTo(e))
                    {
                        e = v;
                        eIsNew = false;
                    }
                }
                if (sIsNew) vertices.Add(s);
                if (eIsNew) vertices.Add(e);
                s.addEdge(new VEdge(s, e));
                e.addEdge(new VEdge(e, s));
            }
            vertices.ForEach(item => visited.Add(false));
            #endregion

            #region 定义广度遍历内部方法
            List<int> BFSTraverse(int vi)
            {
                List<int> inner_res = new List<int>();
                // bfs
                Queue<int> bfsQ = new Queue<int>();
                bfsQ.Enqueue(vi);
                visited[vi] = true;
                while (0 != bfsQ.Count)
                {
                    int curi = bfsQ.Dequeue();
                    inner_res.Add(curi);

                    foreach (var e in vertices[curi].edgeOut)
                    {
                        int id = vertices.IndexOf(e.ev);
                        if (!visited[id])
                        {
                            bfsQ.Enqueue(id);
                            visited[id] = true;
                        }
                    }
                }
                return inner_res;
            }
            #endregion

            #region 定义遍历拓扑关系函数
            List<List<VEdge>> findLoops(List<int> ids)
            {
                List<List<VEdge>> loops = new List<List<VEdge>>();
                int edgeNum = 0;
                foreach (int v in ids)
                {
                    edgeNum += vertices[v].edgeOut.Count;
                    vertices[v].edgeOut.ForEach(e => e.isVisited = false);
                }

                while (edgeNum > 0)
                {
                    VEdge preEdge = null;
                    List<VEdge> newloop = new List<VEdge>();
                    foreach (int v in ids)
                    {
                        foreach (VEdge e in vertices[v].edgeOut)
                            if (!e.isVisited) { preEdge = e; break; }
                        if (preEdge != null) break;
                    }

                    while (true && preEdge != null)
                    {
                        VNode cur_v = preEdge.ev;
                        double theta = preEdge.getInverse().getTheta();

                        VEdge curEdge = preEdge;
                        int i = 0;
                        double ccw_theta = 2 * Math.PI;
                        while (i < cur_v.edgeOut.Count)
                        {
                            double diff_theta = theta - cur_v.edgeOut[i].getTheta();
                            if (diff_theta <= 0) diff_theta += 2 * Math.PI;
                            if (diff_theta <= ccw_theta || cur_v.edgeOut.Count == 1)
                            {
                                ccw_theta = diff_theta;
                                curEdge = cur_v.edgeOut[i];
                            }
                            i++;
                        }
                        preEdge = curEdge;
                        if (preEdge.isVisited)
                        {
                            if (newloop.Count > 0)
                            {
                                loops.Add(newloop);
                                edgeNum -= newloop.Count;
                            }
                            else
                            {
                                curEdge.isVisited = true;
                                edgeNum--;
                            }
                            break;
                        }
                        else
                        {
                            newloop.Add(preEdge);
                            preEdge.isVisited = true;
                        }
                    }
                }
                return loops;
            }
            #endregion

            #region 按照可遍历次数分类
            List<List<int>> sorted_ids = new List<List<int>>();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!visited[i])
                {
                    List<int> record = new List<int>();
                    sorted_ids.Add(BFSTraverse(i));
                }
            }
            #endregion

            #region 找到所有的结果
            foreach (var item in sorted_ids)
            {
                List<List<VEdge>> edges_set = findLoops(item);
                edges_set.ForEach(loop_item =>
                {
                    List<KeyValuePair<TPoint2D, TPoint2D>> loop = new List<KeyValuePair<TPoint2D, TPoint2D>>();
                    loop_item.ForEach(edge => loop.Add(new KeyValuePair<TPoint2D, TPoint2D>(new TPoint2D(edge.sv.pnt.X, edge.sv.pnt.Y), new TPoint2D(edge.ev.pnt.X, edge.ev.pnt.Y))));
                    #region 过滤掉负面积
                    TPoint2DCollection pnts = new TPoint2DCollection();
                    loop.ForEach(edge => { pnts.Add(new TPoint2D(edge.Key)); });
                    if (pnts.Area() > 0)
                        result.Add(loop);
                    #endregion
                });
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 获得点集中所有闭合线框的面积
        /// </summary>
        /// <param name="point_pairs"></param>
        /// <returns></returns>
        public static List<decimal> GetAllLoopsDArea(List<KeyValuePair<TPoint2D, TPoint2D>> point_pairs)
        {
            List<VNode> vertices = new List<VNode>();
            List<bool> visited = new List<bool>();

            #region 构建节点和边数据
            vertices = new List<VNode>();
            int pnt_pair_size = point_pairs.Count;
            List<KeyValuePair<TPoint2D, TPoint2D>> copy_pnts = new List<KeyValuePair<TPoint2D, TPoint2D>>();
            for (int i = 0; i < pnt_pair_size; i++)
            {
                if (CommonUtil.IsEqualPoint(point_pairs[i].Key, point_pairs[i].Value)) continue;
                TPoint2D pnt1 = new TPoint2D(point_pairs[i].Key.X, point_pairs[i].Key.Y);
                TPoint2D pnt2 = new TPoint2D(point_pairs[i].Value.X, point_pairs[i].Value.Y);
                copy_pnts.Add(new KeyValuePair<TPoint2D, TPoint2D>(pnt1, pnt2));
            }

            for (int i = 0; i < copy_pnts.Count; i++)
            {
                KeyValuePair<TPoint2D, TPoint2D> pair = copy_pnts[i];

                double length = pair.Key.DistanceTo(pair.Value);
                if (NumberUtil.CompValue(length, 0, 0.1) == 0) continue;

                VNode s = new VNode(pair.Key);
                VNode e = new VNode(pair.Value);
                bool eIsNew = true, sIsNew = true;
                foreach (VNode v in vertices)
                {
                    if (v.equalTo(s))
                    {
                        s = v;
                        sIsNew = false;
                    }
                    if (v.equalTo(e))
                    {
                        e = v;
                        eIsNew = false;
                    }
                }
                if (sIsNew) vertices.Add(s);
                if (eIsNew) vertices.Add(e);
                s.addEdge(new VEdge(s, e));
                e.addEdge(new VEdge(e, s));
            }
            vertices.ForEach(item => visited.Add(false));
            #endregion

            #region 定义广度遍历内部方法
            List<int> BFSTraverse(int vi)
            {
                List<int> inner_res = new List<int>();
                // bfs
                Queue<int> bfsQ = new Queue<int>();
                bfsQ.Enqueue(vi);
                visited[vi] = true;
                while (0 != bfsQ.Count)
                {
                    int curi = bfsQ.Dequeue();
                    inner_res.Add(curi);

                    foreach (var e in vertices[curi].edgeOut)
                    {
                        int id = vertices.IndexOf(e.ev);
                        if (!visited[id])
                        {
                            bfsQ.Enqueue(id);
                            visited[id] = true;
                        }
                    }
                }
                return inner_res;
            }
            #endregion

            #region 定义遍历拓扑关系函数
            List<List<VEdge>> findLoops(List<int> ids)
            {
                List<List<VEdge>> loops = new List<List<VEdge>>();
                int edgeNum = 0;
                foreach (int v in ids)
                {
                    edgeNum += vertices[v].edgeOut.Count;
                    vertices[v].edgeOut.ForEach(e => e.isVisited = false);
                }

                while (edgeNum > 0)
                {
                    VEdge preEdge = null;
                    List<VEdge> newloop = new List<VEdge>();
                    foreach (int v in ids)
                    {
                        foreach (VEdge e in vertices[v].edgeOut)
                            if (!e.isVisited) { preEdge = e; break; }
                        if (preEdge != null) break;
                    }

                    while (true && preEdge != null)
                    {
                        VNode cur_v = preEdge.ev;
                        double theta = preEdge.getInverse().getTheta();

                        VEdge curEdge = preEdge;

                        int i = 0;
                        double ccw_theta = 2 * Math.PI;
                        while (i < cur_v.edgeOut.Count)
                        {
                            double diff_theta = cur_v.edgeOut[i].getTheta() - theta;
                            if (diff_theta <= 0) diff_theta = 2 * Math.PI + diff_theta;
                            if (diff_theta <= ccw_theta)
                            {
                                ccw_theta = diff_theta;
                                preEdge = cur_v.edgeOut[i];
                            }
                            i++;
                        }
                        if (preEdge.isVisited)
                        {
                            if (newloop.Count > 0)
                            {
                                loops.Add(newloop);
                                edgeNum -= newloop.Count;
                            }
                            else
                            {
                                curEdge.isVisited = true;
                                edgeNum--;
                            }
                            break;
                        }
                        else
                        {
                            newloop.Add(preEdge);
                            preEdge.isVisited = true;
                        }
                    }
                }
                return loops;
            }
            #endregion

            #region 按照可遍历次数分类
            List<List<int>> sorted_ids = new List<List<int>>();
            for (int i = 0; i < vertices.Count; i++)
            {
                if (!visited[i])
                {
                    List<int> record = new List<int>();
                    sorted_ids.Add(BFSTraverse(i));
                }
            }
            #endregion

            #region 找到所有的结果
            List<decimal> result = new List<decimal>();
            foreach (var item in sorted_ids)
            {
                List<List<VEdge>> edges_set = findLoops(item);
                edges_set.ForEach(loop_item =>
                {
                    List<KeyValuePair<TPoint2D, TPoint2D>> loop = new List<KeyValuePair<TPoint2D, TPoint2D>>();
                    loop_item.ForEach(edge => loop.Add(new KeyValuePair<TPoint2D, TPoint2D>(new TPoint2D(edge.sv.pnt.X, edge.sv.pnt.Y), new TPoint2D(edge.ev.pnt.X, edge.ev.pnt.Y))));
                    #region 过滤掉负面积
                    TPoint2DCollection pnts = new TPoint2DCollection();
                    loop.ForEach(edge => { pnts.Add(new TPoint2D(edge.Key)); });
                    decimal DArea = CommonUtil.PntsDArea(pnts);
                    if (DArea > 0)
                        result.Add(DArea);
                    #endregion
                });
            }
            #endregion

            return result;

        }
    }
}
