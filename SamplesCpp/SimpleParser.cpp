#include <iostream>
#include <string>
#include <queue>
#include <tuple>
#include <vector>
#include <functional>
#include <algorithm>
#include <map>

using namespace std;
using str = string;

using u1 = uint8_t;
using u2 = uint16_t;
using u4 = uint32_t;
using u8 = uint64_t;
template<typename T>
using v = vector<T>;

using TT = u2;

#define R(TV, var) TV var; cin >> var;
#define RV(TV, var, cnt) v<TV> var; var.resize(cnt); for (size_t tmpCnt = 0; tmpCnt < cnt; tmpCnt++) cin >> var[tmpCnt];
#define FOR(TV, var, cnt) for (TV var = 0; var < cnt; var++)

void ProcessCase1(TT t)
{
  R(u2, i);
  RV(u2, vi, i);

  cout << "Case #" << t << ": " << *max_element(vi.begin(), vi.end());
}

int main1()
{
  ios_base::sync_with_stdio(false);
  R(TT, T);
  cin >> ws;

  FOR(TT, t, T)
  {
    ProcessCase1(t + 1);
    cout << endl;
  }

  return 0;
}