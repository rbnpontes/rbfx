//
// Copyright (c) 2017-2023 the rbfx project.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

#pragma once
#include <duktape/duktape.h>
#include "../Core/Object.h"
#include "../Math/StringHash.h"
#include "../Core/Variant.h"

#define JS_OBJECT_HEAPPTR_PROP "heapptr"
#define JS_OBJ_HIDDEN_WRAP_CALL DUK_HIDDEN_SYMBOL("__wrapcall")
#define JS_PROP_STRONG_REFS "__refs"

namespace Urho3D
{
    // This file is a set of operations used by the Binding and JavaScript api
    // And must not be used by any other system, components or whatever type like.
    //
    // Note: to prevent collision of names, prefix pattern will be used as prefix of
    // each operation method.

    /// @{
    /// value at obj_idx must be a string hash type.
    /// an exception if value is not string hash compatible.
    /// @}
    URHO3D_API StringHash rbfx_require_string_hash(duk_context* ctx, duk_idx_t stack_idx);
    /// get string hash at stack index
    URHO3D_API StringHash rbfx_get_string_hash(duk_context* ctx, duk_idx_t stack_idx);
    /// push variant to stack
    URHO3D_API void rbfx_push_variant(duk_context* ctx, const Variant& vary);
    URHO3D_API Variant rbfx_get_variant(duk_context* ctx, duk_idx_t stack_idx);
    URHO3D_API VariantMap rbfx_get_variant_map(duk_context* ctx, duk_idx_t stack_idx);
    /// push ref counted object to stack
    URHO3D_API void rbfx_push_object(duk_context* ctx, RefCounted* ref_count);
    /// wrap ref counted instance into js object
    URHO3D_API void rbfx_ref_counted_wrap(duk_context* ctx, duk_idx_t obj_idx, RefCounted* ref_count);
    /// wrap object instance into js object
    URHO3D_API void rbfx_object_wrap(duk_context* ctx, duk_idx_t obj_idx, Object* obj);
    /// @{
    /// get native object ptr from js object
    /// if js object is not valid, a nullptr will be returned
    /// @}
    URHO3D_API RefCounted* rbfx_get_instance(duk_context* ctx, duk_idx_t obj_idx);
    /// set finalizer to ref counted.
    URHO3D_API void rbfx_set_finalizer(duk_context* ctx, duk_idx_t obj_idx, RefCounted* ref_counted);
    URHO3D_API bool rbfx_is_valid_heapptr(duk_context* ctx, void* heapptr);
    /// @{
    /// add heapptr to global stash, this will be used by the engine to retrive
    /// original js instantiated object
    /// @}
    URHO3D_API void rbfx_lock_heapptr(duk_context* ctx, void* heapptr);
    /// @{
    /// remove heapptr from global stash, this usually called after object destruction
    /// @}
    URHO3D_API void rbfx_unlock_heapptr(duk_context* ctx, void* heapptr);

    struct ScriptEventCallDesc
    {
        Object* instance_;
        StringHash eventType_;
        VariantMap& eventArgs_;
    };
    /// @{
    /// call javascript event handler. this method is called when JS subscribes to event.
    /// @}
    URHO3D_API void rbfx_call_event(duk_context* ctx, const ScriptEventCallDesc& evtCall);
    /// @{
    /// move heapptr reference to global stash
    /// making reference strong and not collectable from GC
    /// @}
    URHO3D_API void rbfx_make_strong(duk_context* ctx, void* heapptr);
    /// @{
    /// remove heapptr reference from global stash
    /// making reference weak and collectable from GC
    /// @}
    URHO3D_API void rbfx_make_weak(duk_context* ctx, void* heapptr);
    /// @{
    /// create function with 'this' binding
    /// this method is a low level like ECMA script [Function].bind(this)
    /// @}
    URHO3D_API void rbfx_bind(duk_context* ctx, duk_idx_t func_idx, duk_idx_t this_idx);
}
