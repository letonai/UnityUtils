using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject[] marcadorCamera;
	private int indice;
	public GameObject alvo;
	private Vector3 posJogador;
	public float offsetX,offsetY;
	private bool limitex,maxup,maxdown,maxleft,maxright;
	private Rigidbody2D rdb,rdba,rdbp;
	private float CameraSize,targetSize,size;
	private bool canCloseUp=false;
    public float xMaxDistance, yMaxDistance;

   

    // Use this for initialization
    void Start () {
		//transform.position = marcadorCamera [0].transform.position;
		indice = 0;
		//proximaPos ();
        
		CameraSize = 4;
		targetSize = 2;
        maxdown = false;
        maxup = false;
        maxleft = false;
        maxright = false;
    }

	// Update is called once per frame
	void Update () {
        if (alvo == null) {
            alvo = GameObject.Find("Player");
            return;
        }else{
            rdbp = alvo.transform.GetComponentsInParent<Rigidbody2D>()[0];
            rdba = alvo.transform.GetComponent<Rigidbody2D>();
        }
        Ray2D[] rays = new Ray2D[4];
        Vector2 position = transform.position;
        /*Ray down*/
        rays[0] = new Ray2D(position, Vector2.down);
        /*Ray up*/
        rays[1] = new Ray2D(position, Vector2.up);
        /*Ray left*/
        rays[2] = new Ray2D(position, Vector2.left);
        /*Ray right*/
        rays[3] = new Ray2D(position, Vector2.right);

        /* for (int i = 0; i < 4; i++) {
             RaycastHit2D hit = Physics2D.Raycast(rays[i].origin, rays[i].origin,xMaxDistance);
             if (hit.collider != null && !hit.collider.gameObject.name.Contains("Player")) {
                 Debug.Log("Colidiu: "+hit.collider.gameObject.name); 
             }
         }*/
        //maxdown = maxup = maxleft = maxright = false;

        int mask =  LayerMask.GetMask("Camera");
        
        
        //Debug.Log("Layer: "+ LayerMask.NameToLayer("Camera"));
        RaycastHit2D hit = Physics2D.Raycast(rays[0].origin, rays[0].direction, yMaxDistance, mask);
        if (hit.collider != null && hit.collider.gameObject.tag.Equals("CameraLimit")) {
            maxdown = true;
            Debug.DrawRay(rays[0].origin, rays[0].direction, Color.blue, yMaxDistance);

        } else {
            maxdown = false;
        }

        RaycastHit2D hit1 = Physics2D.Raycast(rays[1].origin, rays[1].direction, yMaxDistance, mask);
        if (hit1.collider != null && hit1.collider.gameObject.tag.Equals("CameraLimit")) {
            maxup = true;
            Debug.DrawRay(rays[1].origin, rays[1].direction, Color.yellow, yMaxDistance);
            
        } else {
            maxup = false;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(rays[2].origin, rays[2].direction, xMaxDistance, mask);
        if (hit2.collider != null && hit2.collider.gameObject.tag.Equals("CameraLimit")) {
            maxleft = true;
            Debug.DrawRay(rays[2].origin, rays[2].direction, Color.red, -xMaxDistance);
        } else {
            maxleft = false;
        }

        RaycastHit2D hit3 = Physics2D.Raycast(rays[3].origin, rays[3].direction, xMaxDistance, mask);
        if (hit3.collider != null && hit3.collider.gameObject.tag.Equals("CameraLimit")) {
            maxright = true;
            
            Debug.DrawRay(rays[3].origin, rays[3].direction, Color.green, xMaxDistance);
        } else {
            maxright = false;
        }



        rdb = rdbp != null?rdbp:rdba;
		posJogador = alvo.transform.position;

        float osx=transform.position.x;// = offsetX * (rdb.velocity.y >= 0 ? 0.3f : -0.3f);
        float osy= transform.position.y;
        float x;//=transform.position.x;
        //Debug.Log(rdb.velocity.y);

        if (rdb.velocity.x>0.01f  && rdb.position.x > transform.position.x && !maxright) {
            osx = Mathf.Lerp(transform.position.x, posJogador.x, Time.deltaTime * .5f);
            //osx = rdb.position.x;
        }  if (rdb.velocity.x < -0.01f && rdb.position.x<transform.position.x && !maxleft) {
            osx = Mathf.Lerp(transform.position.x, posJogador.x, Time.deltaTime * .5f);
            //osx = rdb.position.x;
        }  if (rdb.velocity.y < -0.01f && rdb.position.y < transform.position.y && !maxdown) {
            osy = Mathf.Lerp(transform.position.y, posJogador.y, Time.deltaTime * .5f);
            //osy = rdb.position.y;
        }  if (rdb.velocity.y > 0.01f && rdb.position.y > transform.position.y && !maxup) {
            osy = Mathf.Lerp(transform.position.y, posJogador.y, Time.deltaTime * .5f);
            //osy = rdb.position.y;
        }

		float y = Mathf.Lerp(transform.position.y,posJogador.y+offsetY,Time.deltaTime*10);
		float z = Mathf.Lerp(transform.position.z,transform.position.z,Time.deltaTime*10);
		Vector3 fpos = new Vector3 (osx,osy,z);
		transform.position = fpos;//marcadorCamera [indice].transform.position;
		if(canCloseUp){
			//Debug.Log (CameraSize+" : "+targetSize);
			Camera.main.orthographicSize = Mathf.Lerp(CameraSize,targetSize,Time.time*.02f);
			//Debug.Log (Camera.main.orthographicSize);
			if (Camera.main.orthographicSize < targetSize + .30f) {
				canCloseUp = false;
			}
		}

	}

	public void proximaPos(){
		if (indice == marcadorCamera.Length-1) {
			indice = 0;
		} else {
			indice ++;
		}
	}

	public void selecionaPos(int pos){
		indice = pos;
	}

	public void closeUp(float target){
		Debug.Log ("clouseUp");
		targetSize = target;
		CameraSize=Camera.main.orthographicSize;
		canCloseUp = true;

	}



	void OnTriggerEnter(Collider other) {
		//Debug.Log("Other: "+other.gameObject.name);
		if (other.gameObject.tag.Equals("limitex")) {
			//limitex=false;
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag.Equals("limitex")) {
			//limitex=true;
		}
	}

}
