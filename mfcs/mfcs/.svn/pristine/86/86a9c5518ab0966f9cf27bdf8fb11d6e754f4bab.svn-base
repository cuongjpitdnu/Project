@if (isset($rule) || $errors->has($name))
<div @if (isset($id)) id="{{ $id }}"@endif class="@if (isset($hidden) && $hidden){{ 'd-none' }} @endif" style="display:inline-block;">
<div class="icon-circle tooltip-main" data-toggle="tooltip" data-placement="right" data-html="true"
title="
		@if (isset($rule))
			{{ str_replace(':attribute', $ruleJpName, trans('validation.' . $rule)) }}
		@endif
		@if (isset($name))
			@foreach ($errors->get($name) as $message)
				{{ $message }}
				<br />
			@endforeach
		@endif
"
><i class="fas fa-exclamation text-danger icon-small"></i></div>
</div>
@endif
