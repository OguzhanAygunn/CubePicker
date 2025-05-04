using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace EVERY
{
    public class ObjectInfoHandlerManager : MonoBehaviour
    {
        public static ObjectInfoHandlerManager instance;
        public static ObjectInfoHandlerManager Instance { get { return instance; } }
        [SerializeField] List<ObjectInfoHandler> infoHandlers;
        private void Awake()
        {
            instance = (!instance) ? this : instance;
        }


        [Button(size: ButtonSizes.Large)]
        public void infoHandlersListUpdate()
        {
            infoHandlers = FindObjectsOfType<ObjectInfoHandler>().ToList();
        }

        public static ObjectInfoHandler GetHandler(string id)
        {
            return instance.infoHandlers.Find(handler => handler.Id == id);
        }

        public static GameObject GetObject(string id)
        {
            ObjectInfoHandler handler = GetHandler(id);

            GameObject obj = handler.gameObject;

            return obj;
        }

        public static void AddHandler(ObjectInfoHandler handler)
        {

            if (instance.infoHandlers.Contains(handler))
                return;

            instance.infoHandlers.Add(handler);
        }

        public static void RemoveHandler(ObjectInfoHandler handler)
        {

            if (!instance.infoHandlers.Contains(handler))
                return;

            instance.infoHandlers.Remove(handler);

        }
    }
}
