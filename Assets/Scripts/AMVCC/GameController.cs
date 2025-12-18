using Game;
using SmartMVC;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Controller<GameApplication>
{
    public bool activeKeyboards = true;

    [SerializeField] private Scrollbar verticalScrollbar;

    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 0.6f; // velocità
    [SerializeField] private float smoothTime = 0.08f; // fluidità

    private float scrollVelocity;

    private void Update()
    {
        if (!activeKeyboards || verticalScrollbar == null)
            return;

        float input = 0f;

        if (Input.GetKey(KeyCode.DownArrow))
            input = -1f;

        if (Input.GetKey(KeyCode.UpArrow))
            input = 1f;

        if (input != 0f)
            SmoothScroll(input);
    }

    private void SmoothScroll(float direction)
    {
        float target = verticalScrollbar.value + direction * scrollSpeed * Time.deltaTime;

        verticalScrollbar.value = Mathf.SmoothDamp(
            verticalScrollbar.value,
            Mathf.Clamp01(target),
            ref scrollVelocity,
            smoothTime
        );
    }
}
