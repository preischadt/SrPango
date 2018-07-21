using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject player;
	public GameObject birdOnBranchPrefab;
	public GameObject birdPrefab;
	public GameObject ovniPrefab;
	public GameObject pauseGUI;
	new public Transform camera;
	int previousY;
	float areaHeight = 10f;
	float birding, ovning;
	float birdTime = 0.75f;
	float ovniTime = 2*0.75f;
	int level2y = 20;
	int level3y = 30;
	float diffInc = 0.1f;
	int borderSize = 2;
	int bgBorder = 2;
	bool paused;
	AudioSource[] bg;


	Dictionary<int, ArrayList> instances;

	// Use this for initialization
	void Awake () {
		Unpause();
		bg = GetComponents<AudioSource>();
		instances = new Dictionary<int, ArrayList>();
		previousY = 0;
		Generate(0);
		Generate(1);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			if(paused){
				Unpause();
			}else{
				Pause();
			}
		}
		Time.timeScale = paused? 0f:1f;		

		int y = GetY(player.transform.position.y);
		PlayBGMusic(y);

		//generate static things
		if(y>previousY){
			Remove(previousY-1);
			Generate(previousY+2);
			++previousY;
		}
		if(y<previousY){
			Remove(previousY+1);
			Generate(previousY-2);
			--previousY;
		}
		
		//flutuate difficulty
		float diffMul = 1f;
		if(y>level3y){
			diffMul = 1f + (y-level3y)*diffInc;
		}

		//generate dynamic things
		if(y>level2y-borderSize){ //flying birds
			birding -= Time.deltaTime;
			if(birding<=0){
				birding += birdTime;
				GameObject g = Instantiate(birdPrefab);
				g.GetComponent<FlyingBird>().Initialize(camera.position, player, diffMul);

				//always generate an extra one screen below (for recovers)
				GameObject g2 = Instantiate(birdPrefab);
				g2.GetComponent<FlyingBird>().Initialize(camera.position + Vector3.down*10f, player, diffMul);
			}
		}

		if(y>level3y){ //ovnis
			ovning -= Time.deltaTime;
			if(ovning<=0){
				ovning += ovniTime/diffMul;
				GameObject g = Instantiate(ovniPrefab);
				g.GetComponent<Ovni>().Initialize(camera.position, player, diffMul);
			}
		}
	}

	void Remove(int y){
		ArrayList a;
		if(instances.TryGetValue(y, out a)){
			foreach(GameObject g in a){
				Destroy(g);
			}
			instances.Remove(y);
		}
	}

	void Generate(int y){
		ArrayList a = new ArrayList();
		if(y<level2y){ //generate branchs
			float divisions = 4;
			for(int i=0; i<divisions; i++){
				//randomize direction
				int r = Random.Range(0, 2);
				float lx = r==0? -5.61f : 5.61f;
				float ly = y*areaHeight + i*areaHeight/divisions - 2f;
				if(ly<4f) continue; 
				float sx = r==0? 1f : -1f;
				GameObject g = Instantiate(birdOnBranchPrefab, new Vector3(
					lx,
					ly,
					0f
				), Quaternion.identity);
				g.transform.localScale = new Vector3(sx, 1f, 1f);

				//randomize bird position
				Vector3 pos = g.transform.GetChild(0).localPosition;
				pos.x = Random.Range(0f, 1.5f);
				g.transform.GetChild(0).localPosition = pos;

				a.Add(g);
			}
		}
		instances.Add(y, a);
	}

	void PlayBGMusic(int y){
		if(y<=level2y-bgBorder && !bg[1].isPlaying){
			//play BG 1
			bg[1].Play();
			bg[2].Stop();
		}
		if(y>=level2y && y<=level3y-bgBorder && !bg[2].isPlaying){
			//play BG 2
			bg[1].Stop();
			bg[2].Play();
		}
		if(y>=level3y){
			//stop BGS
			bg[1].Stop();
			bg[2].Stop();
		}
	}

	public int GetY(float ry){
		return Mathf.FloorToInt(ry/areaHeight);
	}

	public int GetLevel2y(){
		return level2y;
	}

	public int GetLevel3y(){
		return level3y;
	}

	public void Pause(){
		paused = true;
		pauseGUI.SetActive(true);
	}

	public void Unpause(){
		paused = false;
		pauseGUI.SetActive(false);
	}

	public void Quit(){
		Application.Quit();
	}

	public void Reset(){
		Application.LoadLevel(Application.loadedLevel);
	}
}
