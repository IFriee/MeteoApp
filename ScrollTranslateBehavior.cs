using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MeteoApp
{
    // Comportement personnalisé qui permet de déplacer une vue (Target) vers le haut ou vers le bas lorsqu'un ScrollView est défilé.
    public class ScrollTranslateBehavior : Behavior<ScrollView>
    {
        private ScrollView _scrollView; // ScrollView pour écouter les événements de défilement.

        public View Target { get; set; } // Vue cible à déplacer.

        // Méthode appelée lorsque le comportement est attaché à un ScrollView.
        // Initialise la propriété _scrollView avec le ScrollView attaché et ajoute un gestionnaire d'événements pour l'événement Scrolled.
        protected override void OnAttachedTo(ScrollView bindable)
        {
            base.OnAttachedTo(bindable);
            _scrollView = bindable;
            _scrollView.Scrolled += OnScrolled;
        }

        // Méthode appelée lorsque le comportement est détaché d'un ScrollView.
        // Supprime le gestionnaire d'événements pour l'événement Scrolled et réinitialise la propriété _scrollView à null.
        protected override void OnDetachingFrom(ScrollView bindable)
        {
            base.OnDetachingFrom(bindable);
            _scrollView.Scrolled -= OnScrolled;
            _scrollView = null;
        }

        // Méthode appelée lorsque l'événement Scrolled est déclenché sur le ScrollView.
        // Déplace la vue cible (Target) vers le haut ou vers le bas en fonction de la position de défilement.
        // Limite la translation maximale à -300 pixels.
        private void OnScrolled(object sender, ScrolledEventArgs e)
        {
            if (Target == null)
            {
                return;
            }

            double scrollPosition = e.ScrollY;
            double translationY = Math.Max(-scrollPosition, -300);
            Target.TranslationY = translationY;
        }
    }
}
