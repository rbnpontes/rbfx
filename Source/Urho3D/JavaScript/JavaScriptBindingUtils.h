// # Generated Header File
#pragma once
#include "JavaScriptBindings.h"

namespace Urho3D {
    StringHash require_string_hash(duk_context* ctx, duk_idx_t obj_idx);
    StringHash get_string_hash(duk_context* ctx, duk_idx_t obj_idx);
    Variant get_variant(duk_context* ctx, duk_idx_t obj_idx);
    void push_variant(duk_context* ctx, const Variant& vary);
    Object* require_object(duk_context* ctx, duk_idx_t obj_idx);
    Object* require_object(duk_context* ctx, duk_idx_t obj_idx, StringHash expected_type);
    Object* get_object(duk_context* ctx, duk_idx_t obj_idx);
    void push_object(duk_context* ctx, Object* instance);
    void push_primitive(duk_context* ctx, duk_idx_t args_count, const ea::string& primitive_name);
    duk_bool_t push_safe_heapptr(duk_context* ctx, void* heapptr);
    void lock_safe_heapptr(duk_context* ctx, void* heapptr);
    void unlock_safe_heapptr(duk_context* ctx, void* heapptr);
}