using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using videosource;

namespace IPCamera
{
	/// <summary>
	/// VideoProvider ��ƵԴ��     
	/// </summary> 
    public class VideoProvider : IComparable    //�̳�IComparable�ӿ� �Ƚ϶��󷵻����͸�������ΪС�ڡ�
	{
		private IVideoSourceDescription	sourceDesc = null;  //��videosource�ӿڻ�õ�����ͨ����ʵ�����ݡ�

		// Name ��������
		public string Name
		{
			get { return sourceDesc.Name; }
		}

		// Description ��������
		public string Description
		{
			get { return sourceDesc.Description; }
		}

		// ProviderName �ṩ���������ԣ�����
		public string ProviderName
		{
			get { return sourceDesc.GetType().ToString(); }
		}

        // ���캯�� ���ݲ�������sourceDesc
		public VideoProvider(IVideoSourceDescription sourceDesc)
		{
			this.sourceDesc = sourceDesc;
		}

		// �Ƚ�����
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			VideoProvider p = (VideoProvider) obj;
			return (this.Name.CompareTo(p.Name));
		}

		// ��ȡ��ƵԴ����ҳ��
		public IVideoSourcePage GetSettingsPage()
		{
			return sourceDesc.GetSettingsPage();
		}

		// ��������
		public void SaveConfiguration(XmlTextWriter writer, object config)
		{
			sourceDesc.SaveConfiguration(writer, config);
		}

		// ��������
		public object LoadConfiguration(XmlTextReader reader)
		{
			return sourceDesc.LoadConfiguration(reader);
		}

		// ������ƵԴ
		public IVideoSource CreateVideoSource(object config)
		{
			return sourceDesc.CreateVideoSource(config);
		}
	}
}
