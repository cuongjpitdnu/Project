var indicatorVisible = false;
(function ($) {
	var indicatorController = {
		__name: 'indicatorController',
		'#indicator click': function () {
			indicatorVisible = true;
			var indicator = this.indicator({
				message: '処理中です。しばらくお待ちください。',
				target: document.body
			}).show();
		},
	};
	$(function () {
		h5.core.controller('#indicator-target', indicatorController);
	});
})(jQuery);
function indicatorHide() {
	if (indicatorVisible) {
		$(".h5-indicator").remove();
		indicatorVisible = false;
	}
}