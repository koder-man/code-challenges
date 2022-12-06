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
using sv = string_view;

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

using TN = u8;

bool IsBoring(TN n)
{
  bool lb = n % 2;
  n /= 10;

  while (n > 0)
  {

  }

  return true;
}

void ProcessCase(TT t)
{
  R(TN, l);
  R(TN, r);
  r++;

  TN total = 0;

  while (l < r)
  {
    TN diff = r - l;
    TN power = 1;

    for (TN inc = 1; inc <= diff; inc *= 10, power *= 5)
    {
      if (l % (inc * 10) || inc * 10 > diff)
      {
        total += power * IsBoring(l);

        l += inc;
        break;
      }
    }
  }

  cout << "Case #" << t << ": " << total;
}

int mainC()
{
  ios_base::sync_with_stdio(false);
  R(TT, T);
  cin >> ws;

  FOR(TT, t, T)
  {
    ProcessCase(t + 1);
    cout << endl;
  }

  return 0;
}