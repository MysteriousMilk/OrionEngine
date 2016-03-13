using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core.Factories
{
    public class ParticlePropertyFactory<TValue>
    {
        private Random _randomizer;

        public ParticlePropertyFactory(Random rand)
        {
            _randomizer = rand;
        }

        public IParticleProperty<TValue> LoadProperty(XElement node)
        {
            string propType = node.Attribute("PropertyType").Value;
            string constType = typeof(Constant<TValue>).GetGenericTypeDefinition().FullName;
            string rangeType = typeof(Range<TValue>).GetGenericTypeDefinition().FullName;
            string randType = typeof(RandomNumber<TValue>).GetGenericTypeDefinition().FullName;

            IParticleProperty<TValue> property = null;

            if (propType.Equals(constType))
            {
                property = new Constant<TValue>();

                if (typeof(TValue) == typeof(float))
                {
                    (property as Constant<TValue>).ConstantValue = (TValue)Convert.ChangeType(
                        XmlConvert.ToSingle(node.Element("ConstantValue").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(double))
                {
                    (property as Constant<TValue>).ConstantValue = (TValue)Convert.ChangeType(
                        XmlConvert.ToDouble(node.Element("ConstantValue").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(int))
                {
                    (property as Constant<TValue>).ConstantValue = (TValue)Convert.ChangeType(
                        XmlConvert.ToInt32(node.Element("ConstantValue").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(Vector2))
                {
                    (property as Constant<TValue>).ConstantValue = (TValue)Convert.ChangeType(
                        ParseVectorNode(node.Element("ConstantValue").Element("Vector")), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(Color))
                {
                    (property as Constant<TValue>).ConstantValue = (TValue)Convert.ChangeType(
                        ParseColorNode(node.Element("ConstantValue").Element("Color")), typeof(TValue)
                        );
                }
            }
            else if (propType.Equals(rangeType))
            {
                property = new Range<TValue>(_randomizer);

                if (typeof(TValue) == typeof(float))
                {
                    (property as Range<TValue>).LowerBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToSingle(node.Element("LowerBound").Value), typeof(TValue)
                        );
                    (property as Range<TValue>).UpperBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToSingle(node.Element("UpperBound").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(double))
                {
                    (property as Range<TValue>).LowerBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToDouble(node.Element("LowerBound").Value), typeof(TValue)
                        );
                    (property as Range<TValue>).UpperBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToDouble(node.Element("UpperBound").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(int))
                {
                    (property as Range<TValue>).LowerBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToInt32(node.Element("LowerBound").Value), typeof(TValue)
                        );
                    (property as Range<TValue>).UpperBound = (TValue)Convert.ChangeType(
                        XmlConvert.ToInt32(node.Element("UpperBound").Value), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(Vector2))
                {
                    (property as Range<TValue>).LowerBound = (TValue)Convert.ChangeType(
                        ParseVectorNode(node.Element("LowerBound").Element("Vector")), typeof(TValue)
                        );
                    (property as Range<TValue>).UpperBound = (TValue)Convert.ChangeType(
                        ParseVectorNode(node.Element("UpperBound").Element("Vector")), typeof(TValue)
                        );
                }
                else if (typeof(TValue) == typeof(Color))
                {
                    (property as Range<TValue>).LowerBound = (TValue)Convert.ChangeType(
                        ParseColorNode(node.Element("LowerBound").Element("Color")), typeof(TValue)
                        );
                    (property as Range<TValue>).UpperBound = (TValue)Convert.ChangeType(
                        ParseColorNode(node.Element("UpperBound").Element("Color")), typeof(TValue)
                        );
                }
            }
            else if (propType.Equals(rangeType))
            {
                property = new RandomNumber<TValue>(_randomizer);
                (property as RandomNumber<TValue>).Scalar = XmlConvert.ToSingle(node.Element("Scalar").Value);
            }

            return property;
        }

        private Vector2 ParseVectorNode(XElement vecNode)
        {
            float x = XmlConvert.ToSingle(vecNode.Attribute("X").Value);
            float y = XmlConvert.ToSingle(vecNode.Attribute("Y").Value);
            return new Vector2(x, y);
        }

        private Color ParseColorNode(XElement colorNode)
        {
            byte r = XmlConvert.ToByte(colorNode.Attribute("R").Value);
            byte g = XmlConvert.ToByte(colorNode.Attribute("G").Value);
            byte b = XmlConvert.ToByte(colorNode.Attribute("B").Value);
            return new Color(r, g, b);
        }
    }
}
