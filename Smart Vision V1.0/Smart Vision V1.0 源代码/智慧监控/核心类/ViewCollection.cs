using System;
using System.Collections;

namespace IPCamera
{
	/// <summary>
	/// ViewCollection ҳ�漯����
	/// </summary>
	public class ViewCollection : CollectionBase
	{
        // ����index��ѯҳ��
		public View this[int index]
		{
			get
			{
				return ((View) InnerList[index]);
			}
		}

        // �������ֻ��ҳ��
        public View GetView(string name)
        {
            foreach (View view in InnerList)
            {
                if ((view.Name == name))
                    return view;
            }
            return null;
        }

		// ����ҳ��ӵ�������
		public void Add(View view)
		{
			InnerList.Add(view);
		}

        // ��ҳ��Ӽ������Ƴ�
		public void Remove(View view)
		{
			InnerList.Remove(view);
		}

	}
}
