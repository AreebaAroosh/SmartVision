using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Text;
using videosource;
using System.Data.OleDb;
using System.Data;


namespace IPCamera
{
	/// <summary>
    /// Configuration Ӧ�ó�������
	/// </summary>
	public class Configuration
	{
		// �����ļ�����
		private string settingsFile;
		private string camerasFile;
		private string viewsFile;

        private string ConStr = "Provider=Microsoft.jet.OLEDB.4.0;Data source=" + System.Windows.Forms.Application.StartupPath + "\\Location.mdb";
        OleDbConnection con;
        OleDbCommand objcmd;
		// ������λ�úʹ�С
		public Point	mainWindowLocation = new Point(100, 50);
		public Size		mainWindowSize = new Size(800, 600);

		//��Ӧ�����ȫ��
		public bool		fitToScreen = true;
		public bool		fullScreen = false;

		// ���
		private int		nextCameraID = 1;
		private int		nextViewID = 1;

        // ����
		public readonly VideoProviderCollection providers = new VideoProviderCollection();
		public readonly CameraCollection cameras = new CameraCollection();
		public readonly ViewCollection views = new ViewCollection();

		// ���캯��
		public Configuration(string path)
		{
			settingsFile = Path.Combine(path, "app.config");        
			camerasFile = Path.Combine(path, "cameras.config");
			viewsFile = Path.Combine(path, "views.config");
            con = new OleDbConnection(ConStr);
		}

		// ��������ͷ
		public void AddCamera(Camera camera)
		{
			camera.ID = nextCameraID++;
			cameras.Add(camera);
			SaveCameras();
		}

		// ƥ������ͷ
		public bool CheckCamera(Camera camera)
		{
			foreach (Camera c in cameras)
			{
				if ((camera.Name == c.Name) && ((camera.ID == 0) || (camera.ID != c.ID)))
					return false;
			}
			return true;
		}

		// ɾ������ͷ
		public bool DeleteCamera(Camera camera)
		{
			cameras.Remove(camera);
			SaveCameras();
			return true;
		}

		// ����ҳ��
		public void AddView(View view)
		{
			view.ID = nextViewID++;
			views.Add(view);
			SaveViews();
		}

        // ����Ƿ��Ѿ�����ҳ�棬����Ϊtrue.
		public bool CheckView(View view)
		{
			foreach (View v in views)
			{
				if ((view.Name == v.Name) && ((view.ID == 0) || (view.ID != v.ID)))
					return false;
			}
			return true;
		}

		// ɾ��ҳ��
		public bool DeleteView(View view)
		{
			views.Remove(view);
			SaveViews();
			return true;
		}

        /// <summary>
        /// ��XML�ļ��б���ͼ�������ͷҳ��������Ϣ
        /// </summary>

        // ��Ӧ�ó�����Ϣ���ص�app.config��
        public void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create);
            XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

            // ���������ɶ���
            xmlOut.Formatting = Formatting.Indented;

            // ��ʼд��
            xmlOut.WriteStartDocument();
            xmlOut.WriteComment("�ǻۼ��ϵͳ�����ļ�");

            // ��Ŀ¼
            xmlOut.WriteStartElement("�ǻۼ��");

            // ������Ŀ¼
            xmlOut.WriteStartElement("MainWindow");
            xmlOut.WriteAttributeString("x", mainWindowLocation.X.ToString());
            xmlOut.WriteAttributeString("y", mainWindowLocation.Y.ToString());
            xmlOut.WriteAttributeString("���", mainWindowSize.Width.ToString());
            xmlOut.WriteAttributeString("�߶�", mainWindowSize.Height.ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteEndElement();
            xmlOut.Close();
        }

        // ��app.config�м���Ӧ�ó���������Ϣ
        public bool LoadSettings()
        {
            bool ret = false;
            if (File.Exists(settingsFile))
            {
                FileStream fs = null;
                XmlTextReader xmlIn = null;

                try
                {
                    // ���ļ�
                    fs = new FileStream(settingsFile, FileMode.Open);
                    // ����XMLreader
                    xmlIn = new XmlTextReader(fs);
                    // ���Կ����ݽڵ�
                    xmlIn.WhitespaceHandling = WhitespaceHandling.None;
                    xmlIn.MoveToContent();

                    // ƥ���Ŀ¼
                    if (xmlIn.Name != "�ǻۼ��")
                        throw new ApplicationException("");

                    // ƥ����һ�ڵ�
                    xmlIn.Read();
                    if (xmlIn.NodeType == XmlNodeType.EndElement)
                        xmlIn.Read();

                    // ƥ��������ڵ�
                    if (xmlIn.Name != "MainWindow")
                        throw new ApplicationException("");

                    // ��������Ϣ
                    int x = Convert.ToInt32(xmlIn.GetAttribute("x"));
                    int y = Convert.ToInt32(xmlIn.GetAttribute("y"));
                    int width = Convert.ToInt32(xmlIn.GetAttribute("���"));
                    int height = Convert.ToInt32(xmlIn.GetAttribute("�߶�"));

                    // ��ѯ��һ�ڵ�
                    xmlIn.Read();
                    if (xmlIn.NodeType == XmlNodeType.EndElement)
                        xmlIn.Read();

                    mainWindowLocation = new Point(x, y);
                    mainWindowSize = new Size(width, height);

                    ret = true;
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (xmlIn != null)
                        xmlIn.Close();
                }
            }
            return ret;
        }


        // ����������ͷĿ¼���浽xml��
		public void SaveCameras()
		{
			// �򿪻򴴽��ļ�
			FileStream		fs = new FileStream(camerasFile, FileMode.Create);
			// ����XmlWriter
			XmlTextWriter	xmlOut = new XmlTextWriter(fs, Encoding.UTF8);
        	
            // �Զ������ʺ��Ķ�
			xmlOut.Formatting = Formatting.Indented;
			// ��ʼд��
			xmlOut.WriteStartDocument();
		
            // ��Ŀ¼��ʼ
			xmlOut.WriteStartElement("Cameras");
			// �������е�����ͷ
			SaveCameras(xmlOut);
			// ��Ŀ¼����
			xmlOut.WriteEndElement();           
		
            // �ر��ļ�
			xmlOut.Close();
		}
		// �ѵ�������ͷ�ڵ���Ϣ�����Xml�ĵ�
		private void SaveCameras(XmlTextWriter writer)
		{
            con.Open();
            string sql1 = "delete * from Location";
            objcmd = new OleDbCommand(sql1, con);
            objcmd.ExecuteNonQuery();
            con.Close();      
			foreach (Camera camera in cameras)
			{
					// �½� "Camera" �ڵ�
					writer.WriteStartElement("Camera");
					// д��ڵ���Ϣ
					writer.WriteAttributeString("id", camera.ID.ToString());
					writer.WriteAttributeString("name", camera.Name);
					writer.WriteAttributeString("desc", camera.Description);

   				    if (camera.Provider != null)
					{
						// д����ƵԴ����
						writer.WriteAttributeString("��ƵԴ", camera.Provider.ProviderName);

						if (camera.Configuration != null)
						{
                            // д����Ƶ������Ϣ
							camera.Provider.SaveConfiguration(writer, camera.Configuration);
						}
					}
					// �ر� "Camera" �ڵ�
					writer.WriteEndElement();
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        string sql = "select count(*) FROM [Location] where ����='" + camera.Name + "' ";        
                        objcmd = new OleDbCommand(sql, con);
                        Int32 id_e = (Int32)objcmd.ExecuteScalar();
                        if (id_e > 0)
                        {
                        }
                        else
                        {
                            try 
                            {
                                switch (camera.Name)
                                {
                                    case "������ʢ�ٽ�������ͷ":
                                        sql = "insert into Location(����,����,��ƵԴ,γ��,����,�ص�) values('" + camera.Name + "','" + camera.Name + "','" + camera.Provider.ProviderName + "','37.300275','-91.05468','����')";
                                        objcmd = new OleDbCommand(sql, con);
                                        objcmd.ExecuteNonQuery();
                                        break;
                                    case "���������������������ѧ����ͷ":
                                        sql = "insert into Location(����,����,��ƵԴ,γ��,����,�ص�) values('" + camera.Name + "','" + camera.Name + "','" + camera.Provider.ProviderName + "','40.463667','-3.74922','���������������������ѧ')";
                                        objcmd = new OleDbCommand(sql, con);
                                        objcmd.ExecuteNonQuery();
                                        break;
                                    case "�Ϻ���ѧD¥����ͷ":
                                        sql = "insert into Location(����,����,��ƵԴ,γ��,����,�ص�) values('" + camera.Name + "','" + camera.Name + "','" + camera.Provider.ProviderName + "','31.314928','121.395085','�Ϻ��б�ɽ���Ϻ���ѧ')";
                                        objcmd = new OleDbCommand(sql, con);
                                        objcmd.ExecuteNonQuery();
                                        break;
                                    default:
                                        sql = "insert into Location(����,����,��ƵԴ) values('" + camera.Name + "','" + camera.Name + "','" + camera.Provider.ProviderName + "')";
                                        objcmd = new OleDbCommand(sql, con);
                                        objcmd.ExecuteNonQuery();
                                        break;
                                }
                            }catch(SystemException)
                            {
                            }       
                        }
                    }
                    con.Close();
			}
		}

		// ��Xml�м�����������ͷĿ¼
		public void LoadCameras()
		{
			// ����ļ��Ƿ����
			if (File.Exists(camerasFile))
			{
				FileStream		fs = null;
				XmlTextReader	xmlIn = null;

				try
				{
					// ���ļ�
					fs = new FileStream(camerasFile, FileMode.Open);
					// ����XMLreader
					xmlIn = new XmlTextReader(fs);
                    // �����հ����ݼ���
					xmlIn.WhitespaceHandling = WhitespaceHandling.None;
					xmlIn.MoveToContent();
					// ����Ŀ¼
                    if (xmlIn.Name != "Cameras")
						throw new ApplicationException("");
					// ��ѯ��һ���ӽڵ�
					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

					// ��������ͷ
					LoadCameras(xmlIn);
				}
				catch (Exception)
				{
				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
		}

        // ��Xml�м��ص�������ͷ�ڵ���Ϣ
        private void LoadCameras(XmlTextReader reader)
        {
            // �������е�����ͷ��Ϣ�ڵ�
            while (reader.Name == "Camera")
            {
                int	depth = reader.Depth;

                // ����������ͷ
                Camera camera = new Camera(reader.GetAttribute("name"));
                camera.ID			= int.Parse(reader.GetAttribute("id"));
                camera.Description	= reader.GetAttribute("desc");
                camera.Provider		= providers.GetProviderByName(reader.GetAttribute("��ƵԴ"));

                // ����������Ϣ
                if (camera.Provider != null)
                {
                    camera.Configuration = camera.Provider.LoadConfiguration(reader);
                }

                // ������ͷ�ӵ�������
                cameras.Add(camera);

                if (camera.ID >= nextCameraID)
                {
                    nextCameraID = camera.ID + 1;
                }

                // ��ѯ��һ�ӽڵ�
                reader.Read();

                // �ƶ�����һ���ڵ�
                while (reader.NodeType == XmlNodeType.EndElement)
                    reader.Read();
                if (reader.Depth < depth)
                    return;
            }
        }


        // ������ҳ��Ŀ¼���浽xml��
		public void SaveViews()
		{
			FileStream		fs = new FileStream(viewsFile, FileMode.Create);
			XmlTextWriter	xmlOut = new XmlTextWriter(fs, Encoding.UTF8);

			xmlOut.Formatting = Formatting.Indented;
			xmlOut.WriteStartDocument();
			xmlOut.WriteStartElement("Views");
			SaveViews(xmlOut);
			xmlOut.WriteEndElement();
			xmlOut.Close();
		}

        // �ѵ���ҳ��ڵ���Ϣ�����Xml�ĵ�
        private void SaveViews(XmlTextWriter writer)
        {
            foreach (View view in views)
            {
                    writer.WriteStartElement("View");
                    writer.WriteAttributeString("id", view.ID.ToString());
                    writer.WriteAttributeString("name", view.Name);
                    writer.WriteAttributeString("desc", view.Description);
                    writer.WriteAttributeString("rows", view.Rows.ToString());
                    writer.WriteAttributeString("cols", view.Cols.ToString());
                    writer.WriteAttributeString("width", view.CellWidth.ToString());
                    writer.WriteAttributeString("height", view.CellHeight.ToString());

                    // д������ͷ
                    string[] strIDs = new string[View.MaxRows * View.MaxCols];
                    for (int i = 0, k = 0; i < View.MaxRows; i++)
                    {
                        for (int j = 0; j < View.MaxCols; j++, k++)
                        {
                            strIDs[k] = view.GetCamera(i, j).ToString();
                        }
                    }
                    writer.WriteAttributeString("cameras", string.Join(",", strIDs));

                    writer.WriteEndElement();
                }
        }

        // ��Xml�м�������ҳ��Ŀ¼
		public void LoadViews()
		{
			if (File.Exists(viewsFile))
			{
				FileStream		fs = null;
				XmlTextReader	xmlIn = null;

				try
				{
					fs = new FileStream(viewsFile, FileMode.Open);
					xmlIn = new XmlTextReader(fs);

					xmlIn.WhitespaceHandling = WhitespaceHandling.None;
					xmlIn.MoveToContent();

					if (xmlIn.Name != "Views")
						throw new ApplicationException("");

					xmlIn.Read();
					if (xmlIn.NodeType == XmlNodeType.EndElement)
						xmlIn.Read();

		         LoadViews(xmlIn);
				}
				catch (Exception)
				{
				}
				finally
				{
					if (xmlIn != null)
						xmlIn.Close();
				}
			}
		}

        // ��Xml�м��ص���ҳ��ڵ���Ϣ
        private void LoadViews(XmlTextReader reader)
        {
            while (reader.Name == "View")
            {
                int	depth = reader.Depth;

                View view = new View(reader.GetAttribute("name"));
                view.ID				= int.Parse(reader.GetAttribute("id"));
                view.Description	= reader.GetAttribute("desc");
                view.Rows			= short.Parse(reader.GetAttribute("rows"));
                view.Cols			= short.Parse(reader.GetAttribute("cols"));
                view.CellWidth		= short.Parse(reader.GetAttribute("width"));
                view.CellHeight		= short.Parse(reader.GetAttribute("height"));

                string[] strIDs = reader.GetAttribute("cameras").Split(',');
                for (int i = 0, k = 0; i < View.MaxRows; i++)
                {
                    for (int j = 0; j < View.MaxCols; j++, k++)
                    {
                        view.SetCamera(i, j, int.Parse(strIDs[k]));
                    }
                }

                views.Add(view);

                if (view.ID >= nextViewID)
                    nextViewID = view.ID + 1;

                // ��ȡ��һ�ڵ�
                reader.Read();
                while (reader.NodeType == XmlNodeType.EndElement)
                    reader.Read();
                if (reader.Depth < depth)
                    return;
            }
        }

	}
}
