db = db.getSiblingDB('MongoDatabaseTest')
db.MAQSTestCollection.drop();
db.MAQSTestCollection.insertMany([
  {
    "lid": "test1",
    "isChanged": true,
    "order": 1
  },
  {
    "lid": "test2",
    "isChanged": false,
    "order": 2
  },
  {
    "lid": "test3",
    "isChanged": false
  },
  {
    "lid": "test4",
    "isChanged": false
  }
])