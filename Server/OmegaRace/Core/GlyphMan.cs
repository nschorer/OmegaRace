using System;
using System.Diagnostics;
using System.Xml;

namespace OmegaRace
{
    class GlyphMan
    {
        private GlyphMan()
        {
            this.pHead = null;
        }

        private static GlyphMan privGetInstance()
        {
            if(pInstance == null)
            {
                pInstance = new GlyphMan();
            }

            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);

            return pInstance;
        }

        public static void AddXml2(String assetName, Azul.Texture pFont)
        {
            // Singleton 
            GlyphMan pMan = GlyphMan.privGetInstance();

            System.Xml.XmlTextReader reader = new XmlTextReader(assetName);

            int key = -1;
            int x = -1;
            int y = -1;
            int width = -1;
            int height = -1;

            // I'm sure there is a better way to do this... but this works for now
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.GetAttribute("key") != null)
                        {
                            key = Convert.ToInt32(reader.GetAttribute("key"));
                        }
                        else if (reader.Name == "x")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    x = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "y")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    y = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "width")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    width = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "height")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    height = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        break;

                    case XmlNodeType.EndElement: //Display the end of the element 
                        if (reader.Name == "character")
                        {
                            // have all the data... so now create a glyph
                           //  Debug.WriteLine("key:{0} x:{1} y:{2} w:{3} h:{4}", key, x, y, width, height);
                             pMan.Add(pFont, key, x, y, width, height);
                        }
                        break;
                }
            }
        }

        public static void AddXml(String assetName, Azul.Texture pFont)
        {
            // Singleton 
            GlyphMan pMan = GlyphMan.privGetInstance();

            System.Xml.XmlTextReader reader = new XmlTextReader(assetName);

            int key = -1;
            int x = -1;
            int y = -1;
            int width = -1;
            int height = -1;

            // I'm sure there is a better way to do this... but this works for now
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.GetAttribute("key") != null)
                        {
                            key = Convert.ToInt32(reader.GetAttribute("key"));
                        }
                        else if (reader.Name == "x")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    x = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "y")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    y = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "width")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    width = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "height")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    height = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        break;

                    case XmlNodeType.EndElement: //Display the end of the element 
                        if (reader.Name == "character")
                        {
                            // have all the data... so now create a glyph
                      //      Debug.WriteLine("key:{0} x:{1} y:{2} w:{3} h:{4}", key, x, y, width, height);
                      //      GlyphMan.Add(glyphName, key, textName, x, y, width, height);
                            pMan.Add(pFont, key, x, y, width, height);

                        }
                        break;
                }
            }

            // Debug.Write("\n");
        }


        private void Add(Azul.Texture pFont, int key, int x, int y, int width, int height)
        {
            Glyph pGlyph = new Glyph(pFont,key, x, y, width, height);

            // quickly add to a linked list
            this.privAddToFront(ref this.pHead, pGlyph);
        }

        public static Glyph Find(int key)
        {
            GlyphMan pMan = GlyphMan.privGetInstance();

            Glyph pNode = pMan.pHead;

            while (pNode != null)
            {
                if( pNode.key == key)
                {
                    // found it
                    break;
                }

                pNode = pNode.pNext;
            }

            return pNode;
        }
        private void privAddToFront(ref Glyph pHead, Glyph pNode)
        {
            // add to front
            Debug.Assert(pNode != null);

            // add node
            if (pHead == null)
            {
                // push to the front
                pHead = pNode;
                pNode.pNext = null;
            }
            else
            {
                // push to front
                pNode.pNext = pHead;
                pHead = pNode;
            }
        }


        // Data:
        private static GlyphMan pInstance;
        private Glyph pHead;
    }
}
