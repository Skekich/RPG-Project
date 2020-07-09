using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float fadeTime = .2f;
        
        private const string defaultSaveFile = "save";

        private SavingSystem saveSys;

        private void Awake()
        {
            saveSys = GetComponent<SavingSystem>();
        }

        private void Start()
        {
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
            StartCoroutine(fader.FadeIn(fadeTime));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }
        
        public void Save()
        {
            saveSys.Save(defaultSaveFile);
        }
        
        public void Load()
        {
            saveSys.Load(defaultSaveFile);
        }

    }
}