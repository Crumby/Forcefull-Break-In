#pragma strict
var target : GameObject;
var speed : float;

var autoDestroyAfter : float;
//prefab for explosion
var explosion : GameObject;

/* Represents the homing sensitivity of the missile.
Range [0.0,1.0] where 0 will disable homing and 1 will make it follow the target like crazy.
This param is fed into the Slerp (defines the interpolation point to pick) */
var homingSensitivity : float = 0.1;

function Start () {
  StartCoroutine(AutoExplode());
}

function Update () {
  target = GameObject.FindGameObjectWithTag("Player");
  if(target != null) {
    var relativePos : Vector3 = target.transform.position - transform.position;
    var rotation : Quaternion = Quaternion.LookRotation(relativePos);

    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
  }

  transform.Translate(0,0,speed * Time.deltaTime,Space.Self);
}

function OnTriggerEnter(other: Collider) {
  //Damage the other gameobject &amp; then destroy self
        ExplodeSelf();
}

private function ExplodeSelf() {
  Instantiate(explosion,transform.position,Quaternion.identity);
  Destroy(gameObject);
}

private function AutoExplode() {
  yield WaitForSeconds(autoDestroyAfter);
  ExplodeSelf();
}