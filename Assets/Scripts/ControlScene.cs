using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScene : MonoBehaviour
{
    private List<ChapterClass> Chapters = new List<ChapterClass>();
    [System.Serializable]
    public class ChapterClass
    {
        public string NameChapter;
        public GameObject Chapter;
        public List<SceneClass> Scenes = new List<SceneClass>();
        [System.Serializable]
        public class SceneClass
        {
            public string NameScene;
            public GameObject Scene;
            public List<ChildSceneClass> ChildsScenes = new List<ChildSceneClass>();
            [System.Serializable]
            public class ChildSceneClass
            {
                public string NameScene;
                public GameObject ChildScene;
            }
        }
    }
    [Header("Tag del Hijo de la escena")][Tooltip("Nombre del tag para que pase independientemente el hijo de las escenas")]
    public string SubChapter;
     int _currentChapter, _currentScene, _currentChildScene;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Chapters.Add(new ChapterClass() { NameChapter = transform.GetChild(i).name, Chapter = transform.GetChild(i).gameObject });
            for (int o = 0; o < transform.GetChild(i).childCount; o++)
            {
                Chapters[i].Scenes.Add(new ChapterClass.SceneClass { NameScene = Chapters[i].Chapter.transform.GetChild(o).name, Scene = Chapters[i].Chapter.transform.GetChild(o).gameObject });

                for (int p = 0; p < transform.GetChild(i).GetChild(o).childCount; p++)
                {
                    if (transform.GetChild(i).GetChild(o).GetChild(p).transform.tag == SubChapter)
                        Chapters[i].Scenes[o].ChildsScenes.Add(new ChapterClass.SceneClass.ChildSceneClass { NameScene = Chapters[i].Scenes[o].Scene.transform.GetChild(p).name, ChildScene = Chapters[i].Scenes[o].Scene.transform.GetChild(p).gameObject });
                }
            }
        }
        UpdateScene();
    }
    void Update()
    {

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.PageDown) /*|| Input.GetMouseButtonDown(1)*/)
        {
            #region NextPage
            if (Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count == 0 && _currentScene < Chapters[_currentChapter].Scenes.Count - 1)
            {
                if (_currentScene < Chapters[_currentChapter].Scenes.Count - 1)
                {
                    _currentScene++;
                    _currentChildScene = 0;
                }
                else if (_currentChapter < Chapters.Count - 1)
                {
                    _currentChapter++;
                    _currentScene = 0;
                }
            }
            else if (_currentChapter < Chapters.Count - 1)
            {
                if (_currentChildScene < Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count - 1) _currentChildScene++;

                else if (_currentScene < Chapters[_currentChapter].Scenes.Count - 1)
                {
                    _currentScene++;
                    _currentChildScene = 0;
                }
                else if (_currentChapter < Chapters.Count - 1)
                {
                    _currentChapter++;
                    _currentScene = 0;
                }
            }
            else
            {
                _currentChapter = 0;
                _currentChildScene = 0;
                _currentScene = 0;
            }
            UpdateScene();
            #endregion
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.PageUp) /*|| Input.GetMouseButtonDown(0)*/)
        {
            #region BackPage
            if (Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count == 0 && _currentScene == 0)
            {
                if (_currentChapter <= 0 && _currentChildScene <= 0)
                {
                    RestartSceneInBack();
                }
                else if (_currentScene > 0)
                {
                    _currentScene--;
                }
                else if (_currentChapter > 0)
                {
                    _currentChapter--;
                    _currentScene = Chapters[_currentChapter].Scenes.Count - 1;
                    _currentChildScene = Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count - 1;
                }
                
                
            }
            else if (_currentChapter >= 0)
            {
                if (_currentChapter <= 0 && _currentChildScene <= 0 && _currentScene <= 0)
                {
                    RestartSceneInBack();
                }
                if (_currentChildScene > 0)
                {
                    _currentChildScene--;
                }
                else if (_currentScene > 0)
                {
                    _currentScene--;
                    _currentChildScene = Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count - 1;
                }
                else if (_currentChapter > 0)
                {
                    _currentChapter--;
                    _currentScene = Chapters[_currentChapter].Scenes.Count - 1;
                }
            }
            UpdateScene();
            #endregion
        }
    }
    public void UpdateScene()
    {
        for (int i = 0; i < Chapters.Count; i++)
        {
            if (i == _currentChapter) Chapters[_currentChapter].Chapter.SetActive(true); else Chapters[i].Chapter.SetActive(false);
            for (int o = 0; o < Chapters[i].Scenes.Count; o++)
            {
                if (o == _currentScene) Chapters[_currentChapter].Scenes[_currentScene].Scene.SetActive(true);
                else Chapters[i].Scenes[o].Scene.SetActive(false);
                for (int p = 0; p < Chapters[i].Scenes[o].ChildsScenes.Count; p++)
                {
                    if (p == _currentChildScene) Chapters[i].Scenes[o].ChildsScenes[p].ChildScene.SetActive(true); else Chapters[i].Scenes[o].ChildsScenes[p].ChildScene.SetActive(false);
                }
            }
        }
    }
    public void RestartScenesInStart()
    {
        _currentChapter = 0;
        _currentChildScene = 0;
        _currentScene = 0;
    }
    public void RestartSceneInBack()
    {
        _currentChapter = Chapters.Count - 1;
        _currentScene = Chapters[_currentChapter].Scenes.Count - 1;
        _currentChildScene = Chapters[_currentChapter].Scenes[_currentScene].ChildsScenes.Count - 1;
    }
}
