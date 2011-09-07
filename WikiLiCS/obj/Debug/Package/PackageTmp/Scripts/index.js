$(function() {
	// GALLERY
	var timeout, clone;

	var animateLoader = function() {
		$('#loader').stop().css('width', 0).animate({
			width: 516
		}, 5000);
	}, loader = $('#loader');

	animateLoader();

	var to = function(node) {
		clearTimeout(timeout);

		if (clone)
			clone.remove();

		var selected = $('#thumbnails li.selected').animate({
			opacity: 0.6
		}, function() {
			selected.removeClass('selected');
		}), img = node.animate({
			opacity: 1
		}, function() {
			node.addClass('selected');
		}).find('img'), src = img.attr('src').replace('_thumb', ''), main = $('#carrousel .main');

		clone = main.clone().css({
			position: 'absolute',
			top: 0,
			left: 0,
			zIndex: 1
		}).insertBefore(main).animate({
			opacity: 0
		}, function() {
			if (clone)
				clone.remove();

			clone = false;
		});

		main.attr('src', src);

		timeout = setTimeout(next, 5000);

		animateLoader();
	};

	var next = function() {
		var selected = $('#thumbnails li.selected'), next = selected.next(), node = next.length ? next : $('#thumbnails li:first');

		to(node);
	};

	// preload thumbnail images
	var i = 0;

	$('#thumbnails img').each(function() {
		var src = this.src.replace('_thumb', '');

		$('<img src="' + src + '"/>').bind('load', function() {
			i++;

			if (i == 7)
				setTimeout(next, 5000);
		});
	});

	// action for thumbnail clicking
	$('#thumbnails a').bind('click', function(e) {
		e.preventDefault();

		var node = $(e.target).parents('li');

		if (!node.hasClass('selected'))
			to(node);
	});

	// ADVICES
	var advicesContainer = $('#advices_alert');

	if (advicesContainer.length) {
		var lang = document.cookie.match(/lang=([a-z]*)/)[1], last = 0;

		var rotateAdvice = function() {
			$.getJSON('/' + lang + '/advices/get_alert/', 'id=' + last, function(response) {
				var id = response.id, title = response.title;

				advicesContainer.html('<img src="/images/advices/' + id + '_thumb.jpg" /><span>' + title + '</span>');

				last = id;
			});
		};

		rotateAdvice();

		setInterval(rotateAdvice, 5000);
	}
});