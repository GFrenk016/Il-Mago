using UnityEngine;

// Nasconde uno sprite di riferimento (es. il muro invisibile del limite mappa,
// disegnato solo per vederlo mentre lavori) non appena il gioco parte, sia in
// Play Mode che nella build finale. Aggiungilo sullo stesso GameObject dello
// Sprite Renderer placeholder: il Collider2D sul GameObject continua a
// funzionare normalmente, solo lo sprite sparisce.
[RequireComponent(typeof(SpriteRenderer))]
public sealed class EditorOnlyVisual : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
