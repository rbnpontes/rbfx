// # Generated File
#pragma once
#include "JavaScriptBindings.h"
#include "../Core/Context.h"
#include "../IO/Log.h"
#include "../Math/Color.h"
#include "../Resource/ResourceCache.h"
#include "../Resource/Resource.h"
#include "../Scene/Serializable.h"
#include "../Scene/Animatable.h"
#include "../UI/UI.h"
#include "../UI/Font.h"
#include "../UI/UIElement.h"
#include "../UI/UISelectable.h"
#include "../UI/Text.h"
namespace Urho3D {
	void Setup_Bindings(duk_context* ctx);
	// Primitive Calls
	void Color_ctor(duk_context* ctx, duk_idx_t obj_idx, const Color& instance);
	Color Color_resolve(duk_context* ctx, duk_idx_t obj_idx);
	void Color_require(duk_context* ctx, duk_idx_t obj_idx);
	void Resource_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void ResourceCache_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void Serializable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void Component_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void Animatable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void Font_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void UIElement_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void UISelectable_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void Text_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
	void UI_ctor(duk_context* ctx, duk_idx_t obj_idx, Object* instance);
}