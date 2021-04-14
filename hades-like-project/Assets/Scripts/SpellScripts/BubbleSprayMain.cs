using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSprayMain : Spell {
    // Start is called before the first frame update
    public GameObject bubbleSpraySub;

    public BubbleSprayMain() {
        manaCost = 55f;
        cooldownTime = 7.0f;
        isBasicAttack = false;
    }

    void Start() {
        transform.position = gunGameObject.transform.position;
        IEnumerator shootFunc = ShootBubble();
        StartCoroutine(shootFunc);
    }

    IEnumerator ShootBubble() {
        for (int i = 0; i < 20; i++) {
            GameObject newBubble = Instantiate(bubbleSpraySub, gunGameObject.transform.position, transform.rotation);
            newBubble.transform.parent = transform.parent;
            Spell spellScript = newBubble.GetComponent<Spell>();
            Vector2 currMouseVec = mouseGameObject.transform.position - gunGameObject.transform.position;
            //print(currMouseVec);
            newBubble.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, Random.Range(-50, 50)) * currMouseVec.normalized * Random.Range(300, 600) * speedMultiplier);
            spellScript.damage = 1f * damageMultiplier;
            spellScript.setPlayerStats(damageMultiplier, playerAgility, playerStrength, playerIntelligence);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
