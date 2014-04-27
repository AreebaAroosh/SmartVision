using System;
using System.Collections;


namespace IPCamera
{
	/// <summary>
    /// CameraCollection ��Ƶ������ 
	/// </summary>
	public class CameraCollection : CollectionBase
	{
        // ����index��ѯCamera
		public Camera this[int index]
		{
			get
			{
				return ((Camera) InnerList[index]);
			}
		}

        // �������ֻ��camera    
        public Camera GetCamera(string name)
        {
            foreach (Camera camera in InnerList)
            {
                if ((camera.Name == name))
                    return camera;
            }
            return null;
        }

        // ����cameraID���camera
        public Camera GetCamera(int cameraID)
        {
            foreach (Camera camera in InnerList)
            {
                if (camera.ID == cameraID)
                    return camera;
            }
            return null;
        }

		// ��camera�ӵ�������
		public void Add(Camera camera)
		{
			InnerList.Add(camera);
		}

        // ��camera�Ӽ������Ƴ�
		public void Remove(Camera camera)
		{
			InnerList.Remove(camera);
		}


	}
}
