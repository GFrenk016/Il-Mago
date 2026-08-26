using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Illumina i tile del contorno invisibile della mappa quando il player si
// avvicina, come avviso visivo del limite di livello. Va su una Tilemap
// "gemella" di Walls_Bounds che contiene lo STESSO identico contorno,
// dipinto con lo stesso tile, ma SENZA Tilemap Collider 2D / Rigidbody 2D /
// Composite Collider 2D: questa tilemap è puramente visiva, la collisione
// resta su Walls_Bounds. All'avvio tutti i tile partono invisibili
// (alpha 0); ogni frame quelli entro warningDistance dal player si accendono
// in base alla distanza, quelli fuori range tornano trasparenti.
[RequireComponent(typeof(Tilemap))]
public sealed class BoundaryGlowEffect : MonoBehaviour
{
    [Header("Colore e raggio dell'avviso")]
    [SerializeField] private Color glowColor = new Color(1f, 0.25f, 0.2f);
    [SerializeField, Min(0.1f)] private float warningDistance = 3f;

    private Tilemap glowTilemap;
    private readonly HashSet<Vector3Int> litCells = new HashSet<Vector3Int>();
    private readonly HashSet<Vector3Int> newLitCells = new HashSet<Vector3Int>();

    private void Awake()
    {
        glowTilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        // Parte tutto spento: sblocca il colore su ogni tile dipinto e lo azzera.
        foreach (Vector3Int cellPos in glowTilemap.cellBounds.allPositionsWithin)
        {
            if (!glowTilemap.HasTile(cellPos))
            {
                continue;
            }

            glowTilemap.SetTileFlags(cellPos, TileFlags.None);
            glowTilemap.SetColor(cellPos, Transparent());
        }
    }

    private void Update()
    {
        Transform player = PlayerHealth.Instance != null ? PlayerHealth.Instance.transform : null;

        if (player == null)
        {
            return;
        }

        Vector3Int playerCell = glowTilemap.WorldToCell(player.position);
        int radiusCells = Mathf.CeilToInt(warningDistance / glowTilemap.cellSize.x) + 1;

        newLitCells.Clear();

        for (int x = -radiusCells; x <= radiusCells; x++)
        {
            for (int y = -radiusCells; y <= radiusCells; y++)
            {
                Vector3Int cellPos = playerCell + new Vector3Int(x, y, 0);

                if (!glowTilemap.HasTile(cellPos))
                {
                    continue;
                }

                Vector3 cellCenter = glowTilemap.GetCellCenterWorld(cellPos);
                float distance = Vector2.Distance(player.position, cellCenter);

                if (distance > warningDistance)
                {
                    continue;
                }

                float intensity = 1f - (distance / warningDistance);
                Color litColor = glowColor;
                litColor.a = intensity;

                glowTilemap.SetTileFlags(cellPos, TileFlags.None);
                glowTilemap.SetColor(cellPos, litColor);
                newLitCells.Add(cellPos);
            }
        }

        // Spegne i tile accesi il frame scorso che ora sono fuori raggio.
        foreach (Vector3Int cellPos in litCells)
        {
            if (!newLitCells.Contains(cellPos))
            {
                glowTilemap.SetColor(cellPos, Transparent());
            }
        }

        litCells.Clear();
        litCells.UnionWith(newLitCells);
    }

    private Color Transparent()
    {
        return new Color(glowColor.r, glowColor.g, glowColor.b, 0f);
    }
}
