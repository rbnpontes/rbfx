// # Generated File
#include "JavaScriptGeneratedBindings.h"
#include "JavaScriptBindingUtils.h"

namespace Urho3D {
	// Primitive Calls
	void Color_ctor(duk_context* ctx, duk_idx_t obj_idx, const Color& instance) {
		Setup_Primitive(ctx, obj_idx, "Color");
		{
			// Setup return var
			float result = 0.0f;
			result = instance.r_;
			// Setup return
			push_variant(ctx, Variant(result));
			duk_put_prop_string(ctx, obj_idx, "r");
		}
		{
			// Setup return var
			float result = 0.0f;
			result = instance.g_;
			// Setup return
			push_variant(ctx, Variant(result));
			duk_put_prop_string(ctx, obj_idx, "g");
		}
		{
			// Setup return var
			float result = 0.0f;
			result = instance.b_;
			// Setup return
			push_variant(ctx, Variant(result));
			duk_put_prop_string(ctx, obj_idx, "b");
		}
		{
			// Setup return var
			float result = 0.0f;
			result = instance.a_;
			// Setup return
			push_variant(ctx, Variant(result));
			duk_put_prop_string(ctx, obj_idx, "a");
		}
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_this(ctx);
			Color value = Color_resolve(ctx, 0);
			Color instance = Color_resolve(ctx, 1);
			
			// Setup return var
			Color result;
			result = instance + value;
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "add");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_this(ctx);
			Color value = Color_resolve(ctx, 0);
			Color instance = Color_resolve(ctx, 1);
			
			// Setup return var
			Color result;
			result = instance - value;
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "sub");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_this(ctx);
			Color value = Color_resolve(ctx, 0);
			Color instance = Color_resolve(ctx, 1);
			
			// Setup return var
			Color result;
			result = instance * value;
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "mul");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_this(ctx);
			Color value = Color_resolve(ctx, 0);
			Color instance = Color_resolve(ctx, 1);
			
			// Setup return var
			bool result = false;
			result = instance == value;
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "equals");
		// setup Color methods.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			unsigned result = 0u;
			result = instance.ToUInt();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "toUInt");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			duk_require_uint(ctx, 0);
			// Setup Arguments
			unsigned arg0 = duk_get_uint_default(ctx, 0, 0u);
			instance.FromUInt(arg0);
			return 0;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "fromUInt");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			duk_require_number(ctx, 0);
			duk_require_number(ctx, 1);
			duk_require_number(ctx, 2);
			duk_require_number(ctx, 3);
			// Setup Arguments
			float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
			float arg1 = (float)duk_get_number_default(ctx, 1, 0.0);
			float arg2 = (float)duk_get_number_default(ctx, 2, 0.0);
			float arg3 = (float)duk_get_number_default(ctx, 3, 0.0);
			instance.FromHSL(arg0, arg1, arg2, arg3);
			return 0;
		}, 4);
		duk_put_prop_string(ctx, obj_idx, "fromHSL");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			duk_require_number(ctx, 0);
			duk_require_number(ctx, 1);
			duk_require_number(ctx, 2);
			duk_require_number(ctx, 3);
			// Setup Arguments
			float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
			float arg1 = (float)duk_get_number_default(ctx, 1, 0.0);
			float arg2 = (float)duk_get_number_default(ctx, 2, 0.0);
			float arg3 = (float)duk_get_number_default(ctx, 3, 0.0);
			instance.FromHSV(arg0, arg1, arg2, arg3);
			return 0;
		}, 4);
		duk_put_prop_string(ctx, obj_idx, "fromHSV");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.SumRGB();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "sumRGB");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Average();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "average");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Luma();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "luma");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Chroma();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "chroma");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Hue();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "hue");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.SaturationHSL();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "saturationHSL");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.SaturationHSV();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "saturationHSV");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Value();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "value");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			Color result;
			result = instance.GammaToLinear();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "gammaToLinear");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			Color result;
			result = instance.LinearToGamma();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "linearToGamma");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Lightness();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "lightness");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.MaxRGB();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "maxRGB");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.MinRGB();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "minRGB");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance.Range();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "range");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			duk_require_boolean(ctx, 0);
			// Setup Arguments
			bool arg0 = duk_get_boolean_default(ctx, 0, false);
			instance.Clip(arg0);
			return 0;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "clip");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			duk_require_boolean(ctx, 0);
			// Setup Arguments
			bool arg0 = duk_get_boolean_default(ctx, 0, false);
			instance.Invert(arg0);
			return 0;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "invert");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Validate Arguments
			Color_require(ctx, 0);
			duk_require_number(ctx, 1);
			// Setup Arguments
			Color arg0= Color_resolve(ctx, 0);
			float arg1 = (float)duk_get_number_default(ctx, 1, 0.0);
			// Setup return var
			Color result;
			result = instance.Lerp(arg0, arg1);
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 2);
		duk_put_prop_string(ctx, obj_idx, "lerp");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			Color result;
			result = instance.Abs();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "abs");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			unsigned result = 0u;
			result = instance.ToUIntArgb();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "toUIntArgb");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color instance = Color_resolve(ctx, __push_idx);
			duk_pop(ctx);
			// Setup return var
			unsigned result = 0u;
			result = instance.ToHash();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_put_prop_string(ctx, obj_idx, "toHash");
	}
	void Color_require(duk_context* ctx, duk_idx_t obj_idx) {
		duk_get_prop_string(ctx, obj_idx, "r");
		duk_require_number(ctx, -1);
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "g");
		duk_require_number(ctx, -1);
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "b");
		duk_require_number(ctx, -1);
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "a");
		duk_require_number(ctx, -1);
		duk_pop(ctx);
	}
	Color Color_resolve(duk_context* ctx, duk_idx_t obj_idx) {
		Color output;
		duk_get_prop_string(ctx, obj_idx, "r");
		float arg0 = (float)duk_get_number_default(ctx, -1, 0.0);
		output.r_ = arg0;
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "g");
		float arg1 = (float)duk_get_number_default(ctx, -1, 0.0);
		output.g_ = arg1;
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "b");
		float arg2 = (float)duk_get_number_default(ctx, -1, 0.0);
		output.b_ = arg2;
		duk_pop(ctx);
		
		duk_get_prop_string(ctx, obj_idx, "a");
		float arg3 = (float)duk_get_number_default(ctx, -1, 0.0);
		output.a_ = arg3;
		duk_pop(ctx);
		
		return output;
	}
	void Resource_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Object_ctor(ctx, obj_idx, instance);
		// Resource - name property.
		duk_push_string(ctx, "name");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Resource* instance = static_cast<Resource*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			ea::string result;
			result = instance->GetName();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Resource* instance = static_cast<Resource*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_string(ctx, 0);
			// Setup Arguments
			const char* arg0 = duk_get_string_default(ctx, 0, "");
			// setter call
			instance->SetName(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
	}
	void ResourceCache_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Object_ctor(ctx, obj_idx, instance);
		// Setup ResourceCache::GetResource(StringHash, string)
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			ResourceCache* instance = static_cast<ResourceCache*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			require_string_hash(ctx, 0);
			duk_require_string(ctx, 1);
			// Setup Arguments
			StringHash arg0 = get_string_hash(ctx, 0);
			const char* arg1 = duk_get_string_default(ctx, 1, "");
			// Setup return var
			Resource* result = nullptr;
			result = instance->GetResource(arg0, arg1);
			// Setup return
			push_object(ctx, result);
			return 1;
		}, 2);
		duk_put_prop_string(ctx, obj_idx, "getResource");
	}
	void Serializable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Object_ctor(ctx, obj_idx, instance);
	}
	void Component_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Serializable_ctor(ctx, obj_idx, instance);
		// Component - enabled property.
		duk_push_string(ctx, "enabled");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Component* instance = static_cast<Component*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			bool result = false;
			result = instance->IsEnabled();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Component* instance = static_cast<Component*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_boolean(ctx, 0);
			// Setup Arguments
			bool arg0 = duk_get_boolean_default(ctx, 0, false);
			// setter call
			instance->SetEnabled(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
	}
	void Animatable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Serializable_ctor(ctx, obj_idx, instance);
	}
	void Font_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Resource_ctor(ctx, obj_idx, instance);
	}
	void UIElement_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Animatable_ctor(ctx, obj_idx, instance);
		// UIElement - enabled property.
		duk_push_string(ctx, "enabled");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			bool result = false;
			result = instance->IsEnabled();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_boolean(ctx, 0);
			// Setup Arguments
			bool arg0 = duk_get_boolean_default(ctx, 0, false);
			// setter call
			instance->SetEnabled(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// UIElement - name property.
		duk_push_string(ctx, "name");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			ea::string result;
			result = instance->GetName();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_string(ctx, 0);
			// Setup Arguments
			const char* arg0 = duk_get_string_default(ctx, 0, "");
			// setter call
			instance->SetName(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// UIElement - children property.
		duk_push_string(ctx, "children");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			ea::vector<SharedPtr<UIElement>> result;
			result = instance->GetChildren();
			// Setup return
			duk_idx_t result_arr_idx = duk_get_top(ctx);
			duk_push_array(ctx);
			for(duk_uarridx_t i = 0; i < result.size(); ++i) {
				SharedPtr<UIElement> result_arr_item= result.at(i);
				push_object(ctx, result_arr_item);
				duk_put_prop_index(ctx, result_arr_idx, i);
			}
			return 1;
		}, 0);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER);
		// UIElement - horizontalAlignment property.
		duk_push_string(ctx, "horizontalAlignment");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			HorizontalAlignment result;
			result = instance->GetHorizontalAlignment();
			// Setup return
			duk_push_int(ctx, result);
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_int(ctx, 0);
			// Setup Arguments
			HorizontalAlignment arg0 = (HorizontalAlignment)duk_get_int(ctx, 0);
			// setter call
			instance->SetHorizontalAlignment(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// UIElement - verticalAlignment property.
		duk_push_string(ctx, "verticalAlignment");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			VerticalAlignment result;
			result = instance->GetVerticalAlignment();
			// Setup return
			duk_push_int(ctx, result);
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_int(ctx, 0);
			// Setup Arguments
			VerticalAlignment arg0 = (VerticalAlignment)duk_get_int(ctx, 0);
			// setter call
			instance->SetVerticalAlignment(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// UIElement - color property.
		duk_push_string(ctx, "color");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			Color_require(ctx, 0);
			// Setup Arguments
			Color arg0= Color_resolve(ctx, 0);
			// setter call
			instance->SetColor(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_SETTER);
		// Setup UIElement::AddChild(UIElement)
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			require_object(ctx, 0, UIElement::GetTypeStatic());
			// Setup Arguments
			UIElement* arg0 = static_cast<UIElement*>(JavaScriptBindings::GetObjectInstance(ctx, 0, UIElement::GetTypeStatic()));
			instance->AddChild(arg0);
			return 0;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "addChild");
		// Setup UIElement::GetColor(Corner)
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_int(ctx, 0);
			// Setup Arguments
			Corner arg0 = (Corner)duk_get_int(ctx, 0);
			// Setup return var
			Color result;
			result = instance->GetColor(arg0);
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_prop_string(ctx, obj_idx, "getColor");
		// Setup UIElement::SetColor(Corner, Color)
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UIElement* instance = static_cast<UIElement*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_int(ctx, 0);
			Color_require(ctx, 1);
			// Setup Arguments
			Corner arg0 = (Corner)duk_get_int(ctx, 0);
			Color arg1= Color_resolve(ctx, 1);
			instance->SetColor(arg0, arg1);
			return 0;
		}, 2);
		duk_put_prop_string(ctx, obj_idx, "setColor");
	}
	void UISelectable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		UIElement_ctor(ctx, obj_idx, instance);
	}
	void Text_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		UISelectable_ctor(ctx, obj_idx, instance);
		// Text - text property.
		duk_push_string(ctx, "text");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			ea::string result;
			result = instance->GetText();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_string(ctx, 0);
			// Setup Arguments
			const char* arg0 = duk_get_string_default(ctx, 0, "");
			// setter call
			instance->SetText(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// Text - font property.
		duk_push_string(ctx, "font");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			Font* result = nullptr;
			result = instance->GetFont();
			// Setup return
			push_object(ctx, result);
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			require_object(ctx, 0, Font::GetTypeStatic());
			// Setup Arguments
			Font* arg0 = static_cast<Font*>(JavaScriptBindings::GetObjectInstance(ctx, 0, Font::GetTypeStatic()));
			// setter call
			instance->SetFont(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// Text - fontSize property.
		duk_push_string(ctx, "fontSize");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			float result = 0.0f;
			result = instance->GetFontSize();
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			duk_require_number(ctx, 0);
			// Setup Arguments
			float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
			// setter call
			instance->SetFontSize(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
		// Setup Text::SetFont(Font, float)
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Text* instance = static_cast<Text*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			require_object(ctx, 0, Font::GetTypeStatic());
			duk_require_number(ctx, 1);
			// Setup Arguments
			Font* arg0 = static_cast<Font*>(JavaScriptBindings::GetObjectInstance(ctx, 0, Font::GetTypeStatic()));
			float arg1 = (float)duk_get_number_default(ctx, 1, 0.0);
			// Setup return var
			bool result = false;
			result = instance->SetFont(arg0, arg1);
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 2);
		duk_put_prop_string(ctx, obj_idx, "setFont");
	}
	void UI_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance) {
		Object_ctor(ctx, obj_idx, instance);
		// UI - root property.
		duk_push_string(ctx, "root");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UI* instance = static_cast<UI*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Setup return var
			UIElement* result = nullptr;
			result = instance->GetRoot();
			// Setup return
			push_object(ctx, result);
			return 1;
		}, 0);
		duk_push_c_function(ctx, [](duk_context* ctx) {
			// Acquire Current Instance
			duk_idx_t obj_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			UI* instance = static_cast<UI*>(get_object(ctx, -1));
			duk_pop(ctx);
			// Validate Arguments
			require_object(ctx, 0, UIElement::GetTypeStatic());
			// Setup Arguments
			UIElement* arg0 = static_cast<UIElement*>(JavaScriptBindings::GetObjectInstance(ctx, 0, UIElement::GetTypeStatic()));
			// setter call
			instance->SetRoot(arg0);
			return 0;
		}, 1);
		duk_def_prop(ctx, obj_idx, DUK_DEFPROP_HAVE_ENUMERABLE | DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_SETTER);
	}
	void Setup_Bindings(duk_context* ctx) {
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call ResourceCache with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			ResourceCache* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type ResourceCache.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new ResourceCache(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<ResourceCache*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor ResourceCache");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			ResourceCache_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "ResourceCache");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call Font with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			Font* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type Font.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new Font(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<Font*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor Font");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			Font_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "Font");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call UIElement with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			UIElement* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type UIElement.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new UIElement(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<UIElement*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor UIElement");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			UIElement_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "UIElement");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call UISelectable with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			UISelectable* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type UISelectable.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new UISelectable(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<UISelectable*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor UISelectable");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			UISelectable_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "UISelectable");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call Text with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			Text* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type Text.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new Text(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<Text*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor Text");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			Text_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "Text");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call UI with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			
			UI* instance = nullptr;
			duk_idx_t obj_idx = duk_get_top(ctx);
			if(obj_idx > 1) {
				URHO3D_LOGERROR("Invalid Constructor Call for Type UI.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_push_this(ctx);
			// If has 1 argument, they must need to be a pointer to object
			if(obj_idx == 0) {
				instance = new UI(JavaScriptBindings::GetContext());
			} else {
				duk_require_pointer(ctx, 0);
				instance = static_cast<UI*>(duk_get_pointer_default(ctx, 0, nullptr));
			}
			
			if(!instance) {
				URHO3D_LOGERROR("Instance is null for Constructor UI");
				return DUK_RET_ERROR;
			}
			instance->AddRef();
			UI_ctor(ctx, obj_idx, instance);
			// Setup Destructor
			Object_Finalizer(ctx, obj_idx, instance);
			// Return this element
			duk_dup(ctx, obj_idx);
			return 1;
		}, DUK_VARARGS);
		duk_put_global_string(ctx, "UI");
		
		// Enumerators
		// setup enum HorizontalAlignment
		duk_push_object(ctx);
		duk_push_int(ctx, HA_LEFT);
		duk_put_prop_string(ctx, -2, "left");
		duk_push_int(ctx, HA_CENTER);
		duk_put_prop_string(ctx, -2, "center");
		duk_push_int(ctx, HA_RIGHT);
		duk_put_prop_string(ctx, -2, "right");
		duk_push_int(ctx, HA_CUSTOM);
		duk_put_prop_string(ctx, -2, "custom");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "HorizontalAlignment");
		// setup enum VerticalAlignment
		duk_push_object(ctx);
		duk_push_int(ctx, VA_TOP);
		duk_put_prop_string(ctx, -2, "top");
		duk_push_int(ctx, VA_CENTER);
		duk_put_prop_string(ctx, -2, "center");
		duk_push_int(ctx, VA_BOTTOM);
		duk_put_prop_string(ctx, -2, "bottom");
		duk_push_int(ctx, VA_CUSTOM);
		duk_put_prop_string(ctx, -2, "custom");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "VerticalAlignment");
		// setup enum Corner
		duk_push_object(ctx);
		duk_push_int(ctx, C_TOPLEFT);
		duk_put_prop_string(ctx, -2, "topLeft");
		duk_push_int(ctx, C_TOPRIGHT);
		duk_put_prop_string(ctx, -2, "topRight");
		duk_push_int(ctx, C_BOTTOMLEFT);
		duk_put_prop_string(ctx, -2, "bottomLeft");
		duk_push_int(ctx, C_BOTTOMRIGHT);
		duk_put_prop_string(ctx, -2, "bottomRight");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "Corner");
		// setup enum Orientation
		duk_push_object(ctx);
		duk_push_int(ctx, O_HORIZONTAL);
		duk_put_prop_string(ctx, -2, "horizontal");
		duk_push_int(ctx, O_VERTICAL);
		duk_put_prop_string(ctx, -2, "vertical");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "Orientation");
		// setup enum FocusMode
		duk_push_object(ctx);
		duk_push_int(ctx, FM_NOTFOCUSABLE);
		duk_put_prop_string(ctx, -2, "notFocusable");
		duk_push_int(ctx, FM_RESETFOCUS);
		duk_put_prop_string(ctx, -2, "resetFocus");
		duk_push_int(ctx, FM_FOCUSABLE);
		duk_put_prop_string(ctx, -2, "focusable");
		duk_push_int(ctx, FM_FOCUSABLE_DEFOCUSABLE);
		duk_put_prop_string(ctx, -2, "focusableDefocusable");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "FocusMode");
		// setup enum LayoutMode
		duk_push_object(ctx);
		duk_push_int(ctx, LM_FREE);
		duk_put_prop_string(ctx, -2, "free");
		duk_push_int(ctx, LM_HORIZONTAL);
		duk_put_prop_string(ctx, -2, "horizontal");
		duk_push_int(ctx, LM_VERTICAL);
		duk_put_prop_string(ctx, -2, "vertical");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "LayoutMode");
		// setup enum TraversalMode
		duk_push_object(ctx);
		duk_push_int(ctx, TM_BREADTH_FIRST);
		duk_put_prop_string(ctx, -2, "breadthFirst");
		duk_push_int(ctx, TM_DEPTH_FIRST);
		duk_put_prop_string(ctx, -2, "depthFirst");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "TraversalMode");
		// setup enum DragAndDropMode
		duk_push_object(ctx);
		duk_push_int(ctx, DD_DISABLED);
		duk_put_prop_string(ctx, -2, "disabled");
		duk_push_int(ctx, DD_SOURCE);
		duk_put_prop_string(ctx, -2, "source");
		duk_push_int(ctx, DD_TARGET);
		duk_put_prop_string(ctx, -2, "target");
		duk_push_int(ctx, DD_SOURCE_AND_TARGET);
		duk_put_prop_string(ctx, -2, "sourceAndTarget");
		duk_seal(ctx, -1);
		duk_put_global_string(ctx, "DragAndDropMode");
		// End Enumerators
		
		
		//setup primitives
		// setup primitive Color
		duk_push_c_function(ctx, [](duk_context* ctx) {
			if (!duk_is_constructor_call(ctx)) {
				URHO3D_LOGERROR("Invalid Constructor Call. Must call Color with 'new' keyword.");
				return DUK_RET_TYPE_ERROR;
			}
			duk_idx_t argc = duk_get_top(ctx);
			Color instance;
			if(argc > 0) {
				// Validate Arguments
				duk_require_number(ctx, 0);
				// Setup Arguments
				float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
				instance.r_ = arg0;
				duk_remove(ctx, 0);
			}
			if(argc > 1) {
				// Validate Arguments
				duk_require_number(ctx, 0);
				// Setup Arguments
				float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
				instance.g_ = arg0;
				duk_remove(ctx, 0);
			}
			if(argc > 2) {
				// Validate Arguments
				duk_require_number(ctx, 0);
				// Setup Arguments
				float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
				instance.b_ = arg0;
				duk_remove(ctx, 0);
			}
			if(argc > 3) {
				// Validate Arguments
				duk_require_number(ctx, 0);
				// Setup Arguments
				float arg0 = (float)duk_get_number_default(ctx, 0, 0.0);
				instance.a_ = arg0;
				duk_remove(ctx, 0);
			}
			duk_idx_t __push_idx = duk_get_top(ctx);
			duk_push_this(ctx);
			Color_ctor(ctx, __push_idx, instance);
			
			return 1;
		}, DUK_VARARGS);
		duk_push_string(ctx, "white");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::WHITE);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "gray");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::GRAY);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "black");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::BLACK);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "red");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::RED);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "green");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::GREEN);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "blue");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::BLUE);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "cyan");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::CYAN);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "magenta");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::MAGENTA);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "yellow");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::YELLOW);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "transparentBlack");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::TRANSPARENT_BLACK);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "luminosityGamma");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::LUMINOSITY_GAMMA);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_push_string(ctx, "luminosityLinear");
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_push_object(ctx);
			Color_ctor(ctx, 0, Color::LUMINOSITY_LINEAR);
			return 1;
		}, 0);
		duk_def_prop(ctx, -3, DUK_DEFPROP_HAVE_GETTER | DUK_DEFPROP_HAVE_ENUMERABLE);
		duk_put_global_string(ctx, "Color");
		// end setup primitives
		
		// setup console object
		duk_push_object(ctx);
		// setup console.log() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			Console_Print(ctx, argc, LOG_DEBUG);
			return 0;
		}, DUK_VARARGS);
		duk_put_prop_string(ctx, -2, "log");
		// setup console.debug() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			Console_Print(ctx, argc, LOG_DEBUG);
			return 0;
		}, DUK_VARARGS);
		duk_put_prop_string(ctx, -2, "debug");
		// setup console.warn() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			Console_Print(ctx, argc, LOG_WARNING);
			return 0;
		}, DUK_VARARGS);
		duk_put_prop_string(ctx, -2, "warn");
		// setup console.error() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			Console_Print(ctx, argc, LOG_ERROR);
			return 0;
		}, DUK_VARARGS);
		duk_put_prop_string(ctx, -2, "error");
		duk_put_global_string(ctx, "console");
		// setup Reflection object
		duk_push_object(ctx);
		// setup Reflection.createObject() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			require_string_hash(ctx, 0);
			// Setup Arguments
			StringHash arg0 = get_string_hash(ctx, 0);
			// Setup return var
			SharedPtr<Object> result;
			result = JavaScriptBindings::GetContext()->CreateObject(arg0);
			// Setup return
			push_object(ctx, result);
			return 1;
		}, 1);
		duk_put_prop_string(ctx, -2, "createObject");
		// setup Reflection.registerComponent() method.
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			duk_require_function(ctx, 0);
			duk_require_string(ctx, 1);
			// Setup Arguments
			duk_idx_t arg0 = 0;
			const char* arg1 = duk_get_string_default(ctx, 1, "");
			Call_RegisterComponent(ctx, arg0, arg1);
			return 0;
		}, 2);
		duk_put_prop_string(ctx, -2, "registerComponent");
		duk_put_global_string(ctx, "Reflection");
		// global defs
		// setup global method getSubsystem
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			require_string_hash(ctx, 0);
			// Setup Arguments
			StringHash arg0 = get_string_hash(ctx, 0);
			// Setup return var
			Object* result = nullptr;
			result = JavaScriptBindings::GetContext()->GetSubsystem(arg0);
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_global_string(ctx, "getSubsystem");
		// setup global method subscribeToEvent
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			require_string_hash(ctx, 0);
			duk_require_function(ctx, 1);
			// Setup Arguments
			StringHash arg0 = get_string_hash(ctx, 0);
			duk_idx_t arg1 = 1;
			Register_Event(ctx, arg1, arg0);
			return 0;
		}, 2);
		duk_put_global_string(ctx, "subscribeToEvent");
		// setup global method unsubscribeFromEvent
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			require_string_hash(ctx, 0);
			duk_require_function(ctx, 1);
			// Setup Arguments
			StringHash arg0 = get_string_hash(ctx, 0);
			duk_idx_t arg1 = 1;
			Remove_Event(ctx, arg1, arg0);
			return 0;
		}, 2);
		duk_put_global_string(ctx, "unsubscribeFromEvent");
		// setup global method toHash
		duk_push_c_function(ctx, [](duk_context* ctx) {
			duk_idx_t argc = duk_get_top(ctx);
			// Validate Arguments
			duk_require_string(ctx, 0);
			// Setup Arguments
			const char* arg0 = duk_get_string_default(ctx, 0, "");
			// Setup return var
			StringHash result;
			result = StringHash(arg0);
			// Setup return
			push_variant(ctx, Variant(result));
			return 1;
		}, 1);
		duk_put_global_string(ctx, "toHash");
		// setup global variables
	}
}