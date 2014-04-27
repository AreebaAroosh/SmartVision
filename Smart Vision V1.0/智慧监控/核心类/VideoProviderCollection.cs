using System;
using System.Collections;
using System.IO;
using System.Reflection;
using videosource;
//@��֮Ե������

namespace IPCamera
{
	/// <summary>
	/// VideoProviderCollection class - �ռ���Ƶ�ṩ��
	/// </summary>
    public class VideoProviderCollection : CollectionBase   //�̳��࣬��InnerList����
	{
		// ���캯��
		public VideoProviderCollection()
		{
		}

		// ������Ż�ȡ��ƵԴ
		public VideoProvider this[int index]
		{
			get
			{
				return ((VideoProvider) InnerList[index]);
			}
		}

		// �������ֻ���ṩ��
		public VideoProvider GetProviderByName(string name)
		{
			foreach (VideoProvider provider in InnerList)
			{
				if (provider.ProviderName == name)
				{
					return provider;
				}
			}
			return null;
		}

		// ��������������������Ƶ�ṩ�̣�ͨ��DLL��������
		public void Load(string path)
		{
            // ʵ����DirectoryInfo��
			DirectoryInfo dir = new DirectoryInfo(path);

			// ��ȡpath������dll�ļ�
			FileInfo[] files = dir.GetFiles("*.dll");

			// ��������dll�ļ�
			foreach (FileInfo f in files)
			{
				LoadAssembly(Path.Combine(path, f.Name));   //�ϲ�·����
			}

            // ���ṩ�̼̳�IComparable����������
			InnerList.Sort();      
		}

		// ���س���
		private void LoadAssembly(string fname)
		{
			Type typeVideoSourceDesc = typeof(IVideoSourceDescription);
			Assembly asm = null;

			try
			{
				// ����assembly����
				asm = Assembly.LoadFrom(fname);

				// ��ȡ����type
				Type[] types = asm.GetTypes();

				// �����������
				foreach (Type type in types)
				{
					// ��ȡ�ӿ�
					Type[] interfaces = type.GetInterfaces();

					// ���type�Ƿ�̳� IVideoSourceDescription
                    if (Array.IndexOf(interfaces, typeVideoSourceDesc) != -1)    //��interfaces��ƥ��typeVideoSourceDesc����
					{
						IVideoSourceDescription	desc = null;

						try
						{
							// ������type��ʵ��
							desc = (IVideoSourceDescription) Activator.CreateInstance(type);
							// �����ṩ�̶���
							InnerList.Add(new VideoProvider(desc));
						}
						catch (Exception)
						{
							
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
