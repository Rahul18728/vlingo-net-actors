// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Actors
{
    public abstract class ActorProxyBase<T>
    {
        public static TNew Thunk<TNew>(ActorProxyBase<T> proxy, Actor actor, TNew arg) => 
            proxy.IsDistributable ? Thunk(actor.LifeCycle.Environment.Stage, arg) : arg;

        public static TNew Thunk<TNew>(Stage stage, TNew arg)
        {
            if (typeof(ActorProxyBase<>).IsAssignableFrom(typeof(TNew)))
            {
                var b = (ActorProxyBase<TNew>) (object) arg!;
                return stage.LookupOrStartThunk<TNew>(Vlingo.Actors.Definition.From(stage, b?.Definition, stage.World.DefaultLogger), b?.Address);
            }

            return arg;
        }

        public ActorProxyBase()
        {
        }
        
        public ActorProxyBase(Definition.SerializationProxy<T> definition, IAddress address)
        {
            Protocol = typeof(T);
            Definition = definition;
            Address = address;
        }

        public Type? Protocol { get; }
        public Definition.SerializationProxy<T>? Definition { get; }
        public IAddress? Address { get; }

        public bool IsDistributable => Address?.IsDistributable ?? false;
    }
}