using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Shared;
using VelcroPhysics.Utilities;

namespace RoyalServer.Game_objects
{
    public enum TipTela
    {
        Player_1,Player_2,Player_3,
        Mob_1,Mob_2,Mob_3,
        Box_1,Box_2,Box_3,Box_4, Box_5, Box_6,
        Bush_1,Bush_2,Bush_3,
        Tree_1,Tree_2,Tree_3,
        Brevna,
        Bullet_1

    }
    public static class BodyConstructor
    {
        public static Body CreateBody(TipTela _type, World _world,ObjectS obj=null,float _Size=1f,float _Rotation=0f)
        {
            Body body;
            switch (_type)
            {
                case TipTela.Player_1:
                    {
                        Vertices bodyvert = new Vertices(8);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-90, 51))* _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-59, -49)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(22, -104)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(60, -63)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(86, 4)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(77, 81)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(9, 105)) * _Size);
                        bodyvert.Add(ConvertUnits.ToSimUnits(new Vector2(-44, 109)) * _Size);

                        PolygonShape playershape = new PolygonShape(bodyvert, 2f);

                        body = BodyFactory.CreateBody(_world);
                        body.CreateFixture(playershape);

                        body.BodyType = BodyType.Dynamic;
                        body.UserData = "Player";
                        body.Restitution = 0.3f;
                        body.Friction = 0.5f;

                    }
                    break;
                case TipTela.Player_2:
                    body = null;
                    break;
                case TipTela.Player_3:
                    body = null;
                    break;
                case TipTela.Mob_1:
                    {
                        body = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(obj.texture.Width / 2 *_Size), 0.2f, ConvertUnits.ToSimUnits(new Vector2(0, 0)), BodyType.Dynamic);
                        body.Restitution = 0.3f;
                        body.Friction = 0.5f;
                        body.UserData = "Zombie";
                    }
                    break;
                case TipTela.Mob_2:
                    body = null;
                    break;
                case TipTela.Mob_3:
                    body = null;
                    break;
                case TipTela.Box_1:
                    body = null;
                    break;
                case TipTela.Box_2:
                    {
                        Vertices vertices1 = new Vertices(4);
                        vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-72, -29)) * _Size);
                        vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-24, -60)) * _Size);
                        vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(19, -8)) * _Size);
                        vertices1.Add(ConvertUnits.ToSimUnits(new Vector2(-37, 29)) * _Size);
                        Vertices vertices2 = new Vertices(4);
                        vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(56, -14)) * _Size);
                        vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(81, 42)) * _Size);
                        vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(22, 64)) * _Size);
                        vertices2.Add(ConvertUnits.ToSimUnits(new Vector2(-6, 12))  *_Size);

                        List<Vertices> boxlist = new List<Vertices>(2);
                        boxlist.Add(vertices1);
                        boxlist.Add(vertices2);
                        body= BodyFactory.CreateCompoundPolygon(_world, boxlist, 2f);
                        body.BodyType= BodyType.Static;
                        body.UserData = "Box_2";
                        body.Rotation = _Rotation;
                    }
                    break;
                case TipTela.Box_3:
                    body = null;
                    break;
                case TipTela.Box_4:
                    body = null;
                    break;
                case TipTela.Box_5:
                    body = null;
                    break;
                case TipTela.Box_6:
                    body = null;
                    break;
                case TipTela.Bush_1:
                    body = null;
                    break;
                case TipTela.Bush_2:
                    body = null;
                    break;
                case TipTela.Bush_3:
                    body = null;
                    break;
                case TipTela.Tree_1:
                    body = null;
                    break;
                case TipTela.Tree_2:
                    body = null;
                    break;
                case TipTela.Tree_3:
                    body = null;
                    break;
                case TipTela.Brevna:
                    body = null;
                    break;
                case TipTela.Bullet_1:
                    {
                        body = BodyFactory.CreateCircle(_world, ConvertUnits.ToSimUnits(obj.texture.Width / 2 * _Size), 0.2f, ConvertUnits.ToSimUnits(new Vector2(0, 0)), BodyType.Dynamic);
                        body.Restitution = 0f;
                        body.Friction = 0.3f;
                        body.UserData = "Bullet_1";
                    }
                    break;
                default:
                    {
                        body = null;
                    }
                    break;
            }






            return body;
        }
    }
}
