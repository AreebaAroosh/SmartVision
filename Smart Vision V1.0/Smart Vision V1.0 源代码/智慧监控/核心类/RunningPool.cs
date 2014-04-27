using System;
using System.Collections;
//@��֮Ե������

namespace IPCamera
{
	/// <summary>
	/// RunningPool
	/// </summary>
	public class RunningPool : CollectionBase
	{
		// �չ��캯��
		public RunningPool()
		{
		}

		// ��ȡ����ͷ
		public Camera this[int index]
		{
			get
			{
				return ((Camera) InnerList[index]);
			}
		}

        // ����camera����ʼ����Ƶ
		public bool Add(Camera camera)
		{
			// ������ƵԴ
			if (camera.CreateVideoSource())
			{
				// ���ӵ�pool��
				InnerList.Add(camera);
				camera.Start();
				return true;
			}
			return false;
		}

		// �Ӽ����Ƴ�Camera���źŹرա�
		public void Remove(Camera camera)
		{
            try                          //ҳ����Ƶ������λ����Ҫ��������
            { 
                camera.SignalToStop();
            }
            catch (Exception )
            {
            
            }
            
			InnerList.Remove(camera);

            // �źŹر�
			
		}
	}
}
