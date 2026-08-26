using UnityEngine;
using UnityEngine.Tilemaps;

// Nasconde il rendering di un'intera Tilemap (es. il layer "muri invisibili"
// disegnato con la Tile Palette per il contorno della mappa) appena il gioco
// parte, sia in Play Mode che nella build finale. Il Tilemap Collider 2D /
// Composite Collider 2D sullo stesso GameObject continuano a funzionare
// normalmente: solo i tile smettono di disegnarsi. Mettilo sul GameObject
// della Tilemap stessa (quello con Tilemap + Tilemap Renderer), non sul Grid.
[RequireComponent(typeof(TilemapRenderer))]
public sealed class EditorOnlyTilemapVisual : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TilemapRenderer>().enabled = false;
    }
}
