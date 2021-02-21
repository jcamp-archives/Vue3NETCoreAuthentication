import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from './index'

@Module({
  name: 'counter',
  store,
  dynamic: true
})
class CounterStore extends VuexModule {
  count = 0

  @Mutation
  increment(delta: number): void {
    this.count += delta
  }

  @Mutation
  decrement(delta: number): void {
    this.count -= delta
  }

  @Mutation
  reset(): void {
    this.count = 0
  }

  // action 'incr' commits mutation 'increment' when done with return value as payload
  @Action({ commit: 'increment' })
  public incr(): number {
    return 1
  }

  // action 'decr' commits mutation 'decrement' when done with return value as payload
  @Action({ commit: 'decrement' })
  public decr(): number {
    return 1
  }
}

export default getModule(CounterStore)
