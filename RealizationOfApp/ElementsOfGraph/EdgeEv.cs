﻿

namespace RealizationOfApp.ElementsOfGraph
{
    public class EdgeEv : EventDrawable
    {
        public Edge edge;
        public bool IsNew = false;
        public VertexGraph startVer;
        public VertexGraph endVer;
        public Arrow arrow;
        public Color BuffColor;
        public EdgeEv(Edge edge, VertexGraph start, ref Arrow arrow)
        {
            this.edge = new(edge);
            BuffColor = edge.GetColor();
            this.arrow = arrow;
            arrow.edge = this;
            startVer = start;
        }
        public EdgeEv(Edge edge, VertexGraph start,VertexGraph end, ref Arrow arrow)
        {
            this.edge = new(edge);
            BuffColor = edge.GetColor();
            this.arrow = arrow;
            arrow.edge = this;
            startVer = start;
            endVer = end;
        }
        public override void MouseMoved(object? source, MouseMoveEventArgs e)
        {
            if (IsAlive && IsNew && source is Application app)
            {
                edge.SetVertex2(e.X, e.Y);
            }
            else if (IsAlive && !IsNew && edge.Contains(e.X, e.Y))
            {
                edge.SetColor(Color.Magenta);
            }
            else
            {
                edge.SetColor(BuffColor);
            }
        }
        public override void MouseButtonPressed(object? source, MouseButtonEventArgs e)
        {
            if (IsAlive && IsNew && e.Button == Mouse.Button.Left && source is Application app)
            {
                foreach (EventDrawable eventDrawable in app.eventDrawables)
                {
                    if (eventDrawable is VertexGraph vertex)
                    {
                        if (vertex.Contains(e.X, e.Y))
                        {
                            IsNew = false;
                            endVer = vertex;
                            edge.SetVertex2(vertex.GetPos());
                            vertex.incindentEdges.Add(this);
                            foreach (EventDrawable eventDrawable1 in app.eventDrawables)
                                eventDrawable1.IsAlive = true;
                            app.graph[startVer.GetString(), vertex.GetString()] = int.Parse(edge.GetWeight());
                            EdgeEv? ev = startVer.incindentEdges.Find(x => x.endVer==startVer && x.startVer==endVer);
                            if (app.IsOriented && app.graph[vertex.GetString(), startVer.GetString()]>0 && ev is not null)
                            {
                                ev.edge.isOriented=false;
                                edge.isOriented = false;
                            }
                            else if(!app.IsOriented)
                            {
                                this.edge.isOriented=false;
                                Arrow arrow = new();
                                EdgeEv edge3 = new(new Edge(new(endVer.GetPos(), Color.Black), new(startVer.GetPos(), Color.Black), "1"), endVer,startVer,ref arrow);
                                edge3.edge.isOriented = false;
                                startVer.incindentEdges.Add(edge3);
                                endVer.incindentEdges.Add(edge3);
                                app.eventDrawables.Insert(app.eventDrawables.Count - 1, arrow);
                                app.eventDrawables.Insert(0, edge3);
                            }
                            app.ColoringComponentsOfConnection();
                            break;
                        }
                    }
                }
            }
            else if (IsAlive && !IsNew && e.Button == Mouse.Button.Right && edge.Contains(e.X, e.Y) && source is Application app2)
            {
                foreach (EventDrawable ev in app2.eventDrawables)
                    ev.IsAlive = false;
                app2.eventDrawables.Add(new EnterTextEdge(edge.GetWeightPosition(), this));
            }
            else if (IsAlive && !IsNew && e.Button == Mouse.Button.Middle && edge.Contains(e.X, e.Y) && source is Application app3)
            {
                IsNeedToRemove = true;
                arrow.IsNeedToRemove = true;
                if (app3.graph[endVer.GetString(), startVer.GetString()]>0 && app3.IsOriented)
                {
                    startVer.incindentEdges.Find(x => x.endVer==startVer && x.startVer==endVer).edge.isOriented=true;
                }
                if(!app3.IsOriented)
                {
                    EdgeEv? edgeEv = startVer.incindentEdges.Find(x => x.endVer==startVer && x.startVer==endVer);
                    if(edgeEv is not null)
                    {
                        edgeEv.IsNeedToRemove = true;
                        edgeEv.arrow.IsNeedToRemove = true;
                    }
                }
            }
        }
        public bool Contains(Vector2f vector)
        {
            return edge.Contains(vector);
        }
        public bool Contains(float x, float y)
        {
            return edge.Contains(x, y);
        }
        public Vector2f GetPosVer1()
        {
            return edge.GetPosVer1();
        }
        public Vector2f GetPosVer2()
        {
            return edge.GetPosVer2();
        }
        public void SetPosVer1(float x, float y)
        {
            edge.SetVertex1(new(x, y));
        }
        public void SetPosVer2(float x, float y)
        {
            edge.SetVertex2(new(x, y));
        }
        public void SetWeight(int weight)
        {
            edge.SetWeight1(weight.ToString());
        }
        public void SetTempColor(Color color)
        {
            edge.SetColor(color);
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            edge.Draw(target, states);
        }
    }
}
