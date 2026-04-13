using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class TileTexture : MonoBehaviour {

    [SerializeField]int  _matNo = 0;
	[SerializeField] Texture[] _texture;
	[SerializeField] string textureName;
	[Range(0, 50)]
	[SerializeField] int _speed;
    MeshRenderer _renderer;
	[SerializeField]private float _currentTex;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (_currentTex < _texture.Length) {
			_currentTex = Mathf.MoveTowards(_currentTex, _texture.Length, _speed*Time.unscaledDeltaTime);
		} else
			_currentTex = 1;
		if (textureName == null) {
			_renderer.sharedMaterials[_matNo].mainTexture = _texture [(int)_currentTex - 1];
		}else
            _renderer.sharedMaterials[_matNo].SetTexture(textureName, _texture[(int)_currentTex -1]);

	}
}
