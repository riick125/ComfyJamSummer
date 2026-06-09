using System.Collections;
using Nez;
using Nez.Sprites;
using Nez.Systems;

namespace ComfyJamSummer.Components.Visuals
{
    public class SimpleFlash : Component, IUpdatable
    {
        //Material to switch to during the flash.
        private Material _flashMaterial;

        private CoroutineManager _coroutineManager;

        // The SpriteRenderer that should flash.
        private SpriteRenderer _spriteAnimator;

        // The material that was in use, when the script started.
        private Material _originalMaterial;

        #region Methods

        #region Unity Callbacks
        public SimpleFlash(Material flashMaterial, SpriteRenderer spriteRenderer)
        {
            _coroutineManager = new CoroutineManager();

            _flashMaterial = flashMaterial;

            // Get the SpriteRenderer to be used,
            // alternatively you could set it from the inspector.
            _spriteAnimator = spriteRenderer;

            // Get the material that the SpriteRenderer uses, 
            // so we can switch back to it after the flash ended.
            _originalMaterial = spriteRenderer.Material;
        }

        #endregion

        public void Flash(float duration = 0.1f)
        {
            if (_coroutineManager == null)
            {
                return;
            }

            _coroutineManager.ClearAllCoroutines();

            // Start the Coroutine, and store the reference for it.
            _coroutineManager.StartCoroutine(FlashRoutine(duration));
        }

        public void Update()
        {
            if (_coroutineManager == null)
            {
                return;
            }

            _coroutineManager.Update();
        }

        private IEnumerator FlashRoutine(float duration)
        {
            // Swap to the flashMaterial.
            _spriteAnimator.Material = _flashMaterial;

            // Pause the execution of this function for "duration" seconds.
            yield return Coroutine.WaitForSeconds(duration);

            // After the pause, swap back to the original material.
            _spriteAnimator.Material = _originalMaterial;
        }

        #endregion
    }
}
